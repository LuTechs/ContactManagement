using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ContactManagement.Models;
using ContactManagement.Services;
using ContactManagement.ViewModel;

namespace ContactManagement.Controllers
{
    public class ContactController : Controller
    {

        private readonly IContactService _contactService;
        public ContactController(IContactService service)
        {
            _contactService = service;
        }

        // GET: Contact
        public async Task<ActionResult> Index(string name, string mobile)
        {
            var contacts = await _contactService.GetAllContactsAsyn(name,mobile);
            ViewBag.NameParm = name;
            ViewBag.MobileParm = mobile;
            return View(contacts);
        }

        // GET: Contact/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View();
        }

        // GET: Contact/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contact/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ContactAddEditViewModel contactAddEditViewModel)
        {
            var validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };
            if (contactAddEditViewModel.PhotoUpload!=null &&!validImageTypes.Contains(contactAddEditViewModel.PhotoUpload.ContentType))
            {
                ModelState.AddModelError("PhotoUpload", "Please choose either a GIF, JPG or PNG image.");
            }

            if (ModelState.IsValid)
            {
                if (contactAddEditViewModel.PhotoUpload != null)
                {
                    var uploadDir = "~/photos";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), contactAddEditViewModel.PhotoUpload.FileName);
                    var imageUrl = Path.Combine(uploadDir, contactAddEditViewModel.PhotoUpload.FileName);
                    contactAddEditViewModel.PhotoUpload.SaveAs(imagePath);
                    contactAddEditViewModel.PhotoPath = imageUrl;
                }
              

                var contact = AutoMapper.Mapper.Map<Contact>(contactAddEditViewModel);
                await _contactService.AddContactAsyn(contact);
                return RedirectToAction("Index");


            }
            return View(contactAddEditViewModel);
        }

        // GET: Contact/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var contact =await _contactService.GetContactAsyn(id);
            var contactAddEditViewModel = AutoMapper.Mapper.Map<ContactAddEditViewModel>(contact);
            return View(contactAddEditViewModel);
        }

        // POST: Contact/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, ContactAddEditViewModel contactAddEditViewModel)
        {
            var validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };
            if (contactAddEditViewModel.PhotoUpload != null &&
                !validImageTypes.Contains(contactAddEditViewModel.PhotoUpload.ContentType))
            {
                ModelState.AddModelError("PhotoUpload", "Please choose either a GIF, JPG or PNG image.");
            }

            if (ModelState.IsValid)
            {
                if (contactAddEditViewModel.PhotoUpload != null)
                {
                    const string uploadDir = "~/photos";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), contactAddEditViewModel.PhotoUpload.FileName);
                    var imageUrl = Path.Combine(uploadDir, contactAddEditViewModel.PhotoUpload.FileName);
                    contactAddEditViewModel.PhotoUpload.SaveAs(imagePath);
                    contactAddEditViewModel.PhotoPath = imageUrl;
                }
              
                AutoMapper.Mapper.CreateMap<Contact, ContactAddEditViewModel>();
                var contact = AutoMapper.Mapper.Map<Contact>(contactAddEditViewModel);
                await _contactService.EditContactAsyn(contact);
                return RedirectToAction("Index");
            }

            return View(contactAddEditViewModel);
        
    }

        // GET: Contact/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var contact = await _contactService.GetContactAsyn(id);
            var contactAddEditViewModel = AutoMapper.Mapper.Map<ContactAddEditViewModel>(contact);
            return View(contactAddEditViewModel);
        }

        // POST: Contact/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteContact(int id)
        {
            try
            {
                var contact = await _contactService.GetContactAsyn(id);
                await _contactService.DeleteContactAsyn(contact);
                string cleanServerPathSymbol = contact.PhotoPath.Substring(contact.PhotoPath.IndexOf('/')+1);
                string filePath = Path.Combine(Server.MapPath("~"), cleanServerPathSymbol);

                if (System.IO.File.Exists(filePath)) 
                {
                    System.IO.File.Delete(filePath);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                throw new HttpException(500,"Server Error");
            }
        }
    }
}
