'use strict';
function PromotionModel(data) {
    if (!data) {
        data = {};
    }
    this.id_ = data.id_ || 0;
    this.Title = data.Title || "";
    this.StartDay = data.StartDay || "";

    this.EndDay = data.EndDay || "";

    this.Discount = data.Discount || 0;
    this.ProductIds = data.ProductIds || "";
    this.ProductCategoryIds = data.ProductCategoryIds || "";
    this.IdUser = data.IdUser || 0;
    this.Active = data.Active || false;
    this.Region = data.Region || "All";

}
function PromotionDetailModel(data) {
    if (!data) {
        data = {};
    }
    this.id_ = data.id_ || 0;
    this.ProductId = data.ProductId || "";
    this.Price = data.Price || 0;
    this.PriceDiscount = data.PriceDiscount || "";
    this.Percent = data.Percent || 100-(data.PriceDiscount * 100 / data.Price).toFixed(0);
    this.Name = data.Name || 0;

}


angular.module('myApp')
    .controller('PromotionCtrl', function($scope, $http, $uibModal) {
        var vm = this;
        vm.promotions = [];

        getAll();


        //methods

        vm.add = add;
        vm.edit = edit;
        vm.remove = remove;


        function add() {
            openCrudDialog(new PromotionModel());
        }

        function edit(o) {
            openCrudDialog(o);
        }

        function remove(o) {
            swal({
                    title: "Xóa khuyến mãi này?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Xóa",
                    cancelButtonText: "Đóng lại",
                    closeOnConfirm: true
                },
                function() {
                    $scope.$applyAsync(function() {
                        $http({
                            method: 'POST',
                            url: "/admin/Promotion/Delete",
                            data: angular.toJson({
                                id_: o.id_
                            })
                        }).then(function onSuccess(response) {
                            vm.promotions = _.reject(vm.promotions, function(n) {
                                return n.id_ === o.id_;
                            });
                            swal({ title: "Đã xóa", type: "success", timer: 1500 });
                        }, function onError(response) {
                        });
                    });
                });
        }

        function openCrudDialog(o) {
            var modalInstance = $uibModal.open({
                templateUrl: 'PromotionCrudDialog.html',
                controller: 'PromotionCrudDialogCtrl',
                controllerAs: 'vm',
                size: "lg",
                backdrop: "static",
                resolve: {
                    promotionModel: o
                }
            });

            modalInstance.result.then(function() {
                getAll();
            }, function() {

            });
        }

        function getAll() {
            $http({
                method: 'GET',
                url: "/admin/Promotion/GetAll"
            }).then(function onSuccess(response) {
                vm.promotions = response.data;

            }, function onError(response) {
            });
        }

    })
    .controller("PromotionCrudDialogCtrl", function($scope,$uibModal, $window, $uibModalInstance, promotionModel, $http) {

        var vm = this;
        vm.form = new PromotionModel(promotionModel);
        
        vm.searchTerm = "";
        vm.numberToDisplay = 20;

        vm.statusList = [
            { text: "Kích hoạt", value: true },
            { text: "Chưa áp dụng", value: false },
        ];
        vm.products = [];

        if (vm.form.id_ !== 0) {
            getPromotionDetails(vm.form.id_);
        }

        //methods
        vm.cancel = close;
        vm.onSelected = onSelected;
        vm.save = save;
        vm.remove = remove;

        vm.onProductAutocompleteSelected = onProductAutocompleteSelected;
        vm.onDiscountChange = onDiscountChange;

        vm.exportProduct = exportProduct;
        vm.openImportDialog = openImportDialog;

        vm.loadMore = loadMore;

        vm.onToggleAll = onToggleAll;

        function onSelected(ids) {
            getProductsByCategoryIds(ids);
        }

        function onToggleAll(ids, selected) {
            console.log("onToggleAll", selected);
            if(selected) {
                getAllProducts();
            }else {
                vm.products = [];
            }
        }

        function onProductAutocompleteSelected(product) {
            var index = _.findIndex(vm.products, function(o) {
                return o.ProductId === product.ProductId;
            });
            if (index !== -1) {
                swal({ title: "Sản phẩm này đã được chọn", type: "warning", timer: 1500 });
            } else {
                var p = new PromotionDetailModel({
                    ProductId: product.ProductId,
                    Price:product.Price,
                    PriceDiscount: product.Price - (product.Price * vm.form.Discount / 100).toFixed(0),
                    Name: product.Name
                });
                vm.products.unshift(p);
            }
        }

        function onDiscountChange() {
            if (vm.form.Discount > 0 && vm.form.Discount <= 100) {
                if (vm.products.length > 0) {
                    angular.forEach(vm.products, function(o) {
                        o.PriceDiscount = o.Price - (o.Price * vm.form.Discount / 100).toFixed(0);
                        o.Percent = vm.form.Discount;
                    });
                }
                console.log("vm.products", vm.products)
            } else {
                swal({ title: "Giảm giá từ 0 đến 100%", timer: 1500, type: "warning" });
            }

        }

        function close() {
            $uibModalInstance.dismiss('cancel');
        }

        function remove(id) {
           
            
            $http({
                method: 'POST',
                url: "/admin/Promotion/RemoveDetail",
                data: angular.toJson({
                    id_:id
                })
            }).then(function onSuccess(response) {
                vm.products = _.reject(vm.products, function (o) {
                    return o.id_ === id;
                });
                swal({ title: "Đã xóa", timer: 1500, type: "success" });
            }, function onError(response) {
               
            });

        }

        function ProductPromotionModel(data) {
            if (!data) {
                data = {};
            }
            this.ProductId = data.ProductId || 0;
            this.CategoryId = data.CategoryId || 0;
            this.Code = data.Code || "";
            this.Name = data.Name || "";

            this.Price = data.Price || 0;
            this.PriceDiscount = data.PriceDiscount || 0;
        }

        function getProductsByCategoryIds(ids) {
            $.blockUI($("body"));
            $http({
                method: 'GET',
                url: "/admin/SanPham/GetProductsByCategoryIds",
                params: {
                    ids: ids
                }

            }).then(function onSuccess(response) {
                var products = [];
                if (response.data.length > 0) {
                    angular.forEach(response.data, function(o) {
                        var product = new PromotionDetailModel({
                            ProductId: o.ProductId,
                            Price: o.Price,
                            PriceDiscount: o.Price - (o.Price * vm.form.Discount / 100).toFixed(0),
                            Name: o.Name,
                            Percent: vm.form.Discount
                        });
                        products.push(product);
                    });
                }
                vm.products = products;
                $.unblockUI();
            }, function onError(response) {
                $.unblockUI();
            });
        }

        function getProductsByIds(productIds) {
            $.blockUI($("body"));
            $http({
                method: 'POST',
                url: "/admin/SanPham/GetProductsByIds",
                data:angular.toJson({
                    ids: productIds
                })

            }).then(function onSuccess(response) {
                var products = [];
                if (response.data.length > 0) {
                    angular.forEach(response.data, function(o) {
                        var product = new PromotionDetailModel({
                            ProductId: o.ProductId,
                            Price: o.Price,
                            PriceDiscount: o.Price - (o.Price * vm.form.Discount / 100).toFixed(0),
                            Name: o.Name,
                            Percent: vm.form.Discount
                        });
                        products.push(product);
                    });
                }
                vm.products = products;
                $.unblockUI();
            }, function onError(response) {
                $.unblockUI();
            });
        }
        
        function getPromotionDetails(promotionId) {
            $.blockUI($("body"));
            $http({
                method: 'GET',
                url: "/admin/Promotion/GetPromotionDetails",
                params: {
                    promotionId: promotionId
                }

            }).then(function onSuccess(response) {
                var products = [];
                if (response.data.length > 0) {
                    angular.forEach(response.data, function (o) {
                        var product = new PromotionDetailModel(o);
                        products.push(product);
                    });
                }
                vm.products = products;
                $.unblockUI();
            }, function onError(response) {
                $.unblockUI();
            });
        }
        
        function getAllProducts() {
            $.blockUI($("body"));
            $http({
                method: 'POST',
                url: "/admin/SanPham/GetAllProducts",

            }).then(function onSuccess(response) {
                var products = [];
                if (response.data.length > 0) {
                    angular.forEach(response.data, function (o) {
                        var product = new PromotionDetailModel({
                            ProductId: o.ProductId,
                            Price: o.Price,
                            PriceDiscount: o.Price - (o.Price * vm.form.Discount / 100).toFixed(0),
                            Name: o.Name,
                            Percent: vm.form.Discount
                        });
                        products.push(product);
                    });
                }
                vm.products = products;
                $.unblockUI();
            }, function onError(response) {
                $.unblockUI();
            });
        }

        function save() {

            if (vm.form.StartDay === "") {
                swal({ title: "Chưa chọn ngày bắt đầu", timer: 1500, type: "warning" });
                return;
            }
            if (vm.form.EndDay === "") {
                swal({ title: "Chưa chọn ngày kết thúc", timer: 1500, type: "warning" });
                return;
            }
            if (vm.form.Discount < 0 || vm.form.Discount > 100) {
                swal({ title: "Giảm giá từ 0 đến 100%", timer: 1500, type: "warning" });
                return;
            }
            if (vm.products.length === 0) {
                swal({ title: "Chưa chọn sản phẩm khuyến mãi", timer: 1500, type: "warning" });
                return;
            }
            $.blockUI($("body"));
            var ids = _.pluck(vm.products, "ProductId");
            vm.form.ProductIds = ids.join(",");

            var details = [];
            angular.forEach(vm.products, function (o) {
                details.push({
                    id_: o.id_,
                    ProductId: o.ProductId,
                    PriceDiscount: o.PriceDiscount,
                    Percent: o.Percent,
                });
            });

            var model = {
                id_: vm.form.id_,
                Title: vm.form.Title,
                StartDay: vm.form.StartDay,
                EndDay: vm.form.EndDay,
                Discount: vm.form.Discount,
                ProductCategoryIds: vm.form.ProductCategoryIds,
                ProductIds: vm.form.ProductIds,
                Active: vm.form.Active,
                Region: vm.form.Region,
                Details: angular.toJson(details)
            };

            $http({
                method: 'POST',
                url: "/admin/Promotion/Save",
                data: angular.toJson(model)
            }).then(function onSuccess(response) {
                vm.form.id_ = response.data.PromotionId;
                $.unblockUI();
                $uibModalInstance.close();
                swal({ title: "Đã tạo khuyến mãi", timer: 1500, type: "success" });
            }, function onError(response) {
                $.unblockUI();
            });
        }

        function exportProduct() {
            if (vm.form.id_ === 0) {
                swal({ title: "Lưu trước khi xuất file", timer: 1500, type: "warning" });
                return;
            }
            var w = $window.innerWidth - 50,
                h = $window.innerHeight - 50;

            var winprops = 'resizable=0, height=' + h + ',width=' + w + ',top=50' + ',left=0';
            winprops += ',scrollbars=1';
            winprops += ',fullscreen=yes';
            $window.open("/admin/Promotion/Export?promotionId=" + vm.form.id_, "_blank", winprops);
        }

        function openImportDialog() {
            var modalInstance = $uibModal.open({
                templateUrl: 'PromotionImportDialog.html',
                controller: 'PromotionImportDialogCtrl',
                controllerAs: 'vm',
                size: "sm",
                backdrop: "static",
                resolve: {
                    
                }
            });

            modalInstance.result.then(function (products) {
                console.log("products", products);
                vm.products = products;
            }, function () {

            });
        }

        function loadMore () {
            if (vm.numberToDisplay + 5 < vm.products.length) {
                vm.numberToDisplay += 5;
            } else {
                vm.numberToDisplay = vm.products.length;
            }
        };

    })
    .controller("PromotionImportDialogCtrl", function($scope, $uibModalInstance, FileUploader, $http) {
        var vm = this;
        vm.progressValue = 0;
        vm.showProgress = false;

        vm.uploader = new FileUploader({
            url: "/admin/Promotion/UploadImportFile",

            filters: [
                {
                    name: 'customFilter',
                    fn: function(item) {
                        var ext = item.name.split('.').pop(),
                            fileExts = ['xls', 'xlsx'];
                        console.log("check", _.contains(fileExts, angular.lowercase(ext)))
                        return _.contains(fileExts, angular.lowercase(ext));

                    }
                }
            ]
        });

        vm.uploader.onAfterAddingAll = function() {
            vm.showProgress = true;
        };
        vm.uploader.onProgressAll = function(progress) {
            vm.progressValue = progress;
        };

        vm.uploader.onSuccessItem = function (item, response) {
            $.unblockUI();
            var products = angular.fromJson(response.products);
            swal({ title: "Đã  import sản phẩm khuyến mãi", type: "success", timier: 1500 });
            $uibModalInstance.close(products);
        };
        vm.uploader.onCompleteAll = function() {
            vm.showProgress = false;
        };

        vm.uploader.onComplete = function (response, status, headers) {
            console.log("onComplete", response)
        };

        //methods
        vm.onUpload = upload;
    vm.close = close;

    function upload() {
        $.blockUI($("body"));
            vm.uploader.uploadAll();
        }
        function close() {
            $uibModalInstance.dismiss('cancel');
        }
    });



