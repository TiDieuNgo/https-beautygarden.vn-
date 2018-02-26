(function () {
    'use strict';

    angular
        .module('myApp')
        .component('cpProductCategoryTree', {
            templateUrl: '/Scripts/app/Components/product-category-tree/view.html',
            controller: cpProductCategoryTreeComponent,
            controllerAs: "vm",
            bindings: {
                productCategoryIds: "=",
                onSelected: "&",
                onToggleAll:"&"
            }
        });

    /** @ngInject */
    function cpProductCategoryTreeComponent($http) {
        var vm = this;


        vm.$onInit = function () {
            vm.status = {
                isOpen:false
            };
            vm.selectedAll = false;
            vm.datas = [];
            vm.namesSelected = "Chọn danh mục...";
            vm.isSelectAll = false;
            getDatas();
        };

        vm.$onDestroy = function () {
            vm = null;
        };

        //methods
        vm.onSelectedChange = onSelectedChange;
        vm.toggleSelectedAll = toggleSelectedAll;
        vm.onSelectedAllChange = onSelectedAllChange;

        function toggleSelectedAll() {
            vm.selectedAll = !vm.selectedAll;
            vm.isSelectAll = true;
            onSelectedAllChange();
        }

        function onSelectedAllChange() {
            angular.forEach(vm.datas, function(l1) {
                l1.Selected = vm.selectedAll;
                if (l1.Childs.length > 0) {
                    angular.forEach(l1.Childs, function(l2) {
                        l2.Selected = vm.selectedAll;
                        if (l2.Childs.length > 0) {
                            angular.forEach(l2.Childs, function (l3) {
                                l3.Selected = vm.selectedAll;
                            });
                        }
                    });
                    
                }
            });
            
            vm.isSelectAll = true;
            getSelected(true);
        }

        function CategoryModel(data) {
            if (!data) data = {};
            this.DanhMucId = data.DanhMucId || 0;
            this.Name = data.Name || "";
            this.Level = data.Level || 0;
            this.ParentId = data.ParentId || 0;
            this.SapXepDanhMuc = data.SapXepDanhMuc || 0;
            this.Selected = data.Selected || false;
            this.Active = data.Active || false;

            this.Childs = data.Childs || [];
        }

        function onSelectedChange(item) {
            if (item.Childs.length > 0) {
                angular.forEach(item.Childs, function(l2) {
                    l2.Selected = item.Selected;
                    if (l2.Childs.length > 0) {
                        angular.forEach(l2.Childs, function(l3) {
                            l3.Selected = l2.Selected;
                        });
                    }
                });
            }
            getSelected(true);
        }

        function getSelected(callbackFuc) {
            vm.namesSelected = "";
            var ids = [];
            var names = [];
            if (vm.datas.length > 0) {
                angular.forEach(vm.datas, function(l1) {
                    if (l1.Selected) {
                        ids.push(l1.DanhMucId);
                        names.push(l1.Name);
                    }
                    if (l1.Childs.length > 0) {
                        angular.forEach(l1.Childs, function (l2) {
                            if (l2.Selected) {
                                ids.push(l2.DanhMucId);
                                names.push(l2.Name);
                            }
                            if (l2.Childs.length > 0) {
                                angular.forEach(l2.Childs, function(l3) {
                                    if (l3.Selected) {
                                        ids.push(l3.DanhMucId);
                                        names.push(l3.Name);
                                    }
                                });
                            }
                        });
                    }
                });
            }
          
            vm.productCategoryIds = ids.join(",");

            if (names.length > 3) {
                var threeNames = [];
                angular.forEach(names, function (n, i) {
                    
                    if (i < 3)
                        threeNames.push(n);
                });
                vm.namesSelected = threeNames.join(", ") + "...và " + (names.length - 3) + " danh mục khác.";
            } else {
                vm.namesSelected = names.join(", ");
            }

            if (vm.namesSelected === "")
                vm.namesSelected = "Chọn danh mục...";

            if (callbackFuc) {
                if (vm.isSelectAll) {
                    console.log("isSelectAll", vm.isSelectAll);
                    vm.onToggleAll({ productCategoryIds: vm.productCategoryIds, selectAll: vm.selectedAll });
                }else {
                    vm.onSelected({ productCategoryIds: vm.productCategoryIds });
                }
            }
                
        }

        function getDatas() {
            $http({
                method: 'GET',
                url: "/admin/DanhMucSanPham/GetCategoriesTreeview"
               
            }).then(function onSuccess(response) {
                var categoryIds = vm.productCategoryIds.split(",") || [];
               
                
                var productCategories = response.data.ProductCategories;
                var categoriesLevel1 = _.filter(productCategories, function (o) {
                    return o.Level === 1;
                });

                if (categoriesLevel1.length > 0) {
                    angular.forEach(categoriesLevel1, function(l1) {
                        var category1 = new CategoryModel({
                            DanhMucId: l1.DanhMucId,
                            Name: l1.Name,
                            SapXepDanhMuc: l1.SapXepDanhMuc,
                            Selected: _.contains(categoryIds, l1.DanhMucId+""),
                            Childs: []
                        });

                        var categoriesLevel2 = _.filter(productCategories, function (l2) {
                            return l2.ParentId === l1.DanhMucId;
                        });
                        if (categoriesLevel2.length > 0) {
                            angular.forEach(categoriesLevel2, function (l2) {
                                var category2 = new CategoryModel({
                                    DanhMucId: l2.DanhMucId,
                                    Name: l2.Name,
                                    SapXepDanhMuc: l2.SapXepDanhMuc,
                                    Selected: _.contains(categoryIds, l2.DanhMucId + ""),
                                    Childs: []
                                });

                                var categoriesLevel3 = _.filter(productCategories, function (l3) {
                                    return l3.ParentId === l2.DanhMucId;
                                });
                                if (categoriesLevel3.length > 0) {
                                    angular.forEach(categoriesLevel3, function (l3) {
                                        var category3 = new CategoryModel({
                                            DanhMucId: l3.DanhMucId,
                                            Name: l3.Name,
                                            SapXepDanhMuc: l3.SapXepDanhMuc,
                                            Selected: _.contains(categoryIds, l3.DanhMucId + ""),
                                            Childs: []
                                        });

                                        category2.Childs.push(category3);
                                    });
                                }


                                category1.Childs.push(category2);
                            });
                        }

                        vm.datas.push(category1);
                    });
                }
                getSelected(false);
            }, function onError(response) {
               
            });
        }


    }
})();