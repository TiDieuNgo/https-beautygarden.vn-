using System.Web.Mvc;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Data.Infrastructure;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Core.Common;


namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var layDl = _userRepository.GetAll();
            return View(layDl);
        }
        public ActionResult Add()
        {
            return View("Create");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNew(User postlen)
        {
            User user = new User()
            {
                UserId = postlen.UserId,
                Email = postlen.Email,
                HoTen = postlen.HoTen,
                TenDangNhap = postlen.TenDangNhap,
                MatKhau = Md5Encrypt.Md5EncryptPassword(postlen.MatKhau),
                NgayTao = postlen.NgayTao,
                TrangThai = postlen.TrangThai,

            };
            _userRepository.Add(user);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            var dlcu = _userRepository.GetById(id);

            return View("Edit", dlcu);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(User form)
        {
            User dlcu = _userRepository.GetById(form.UserId);
            dlcu.UserId = form.UserId;
            dlcu.Email = form.Email;
            dlcu.HoTen = form.HoTen;
            dlcu.TenDangNhap = form.TenDangNhap;
            dlcu.MatKhau = Md5Encrypt.Md5EncryptPassword(form.MatKhau);
            dlcu.NgayTao = form.NgayTao;
            dlcu.TrangThai = form.TrangThai;
            _userRepository.Update(dlcu);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var sanpham = _userRepository.GetById(id);
            _userRepository.Delete(sanpham);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult About()
        {
            return View();
        }
    }
}

