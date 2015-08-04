using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ContactManagement.Controllers;
using ContactManagement.Mapper;
using ContactManagement.Services;
using ContactManagement.ViewModel;
using HtmlAgilityPack;
using Moq;
using RazorGenerator.Testing;
using Xunit;
using Xunit.Sdk;

namespace ContactManagement.Tests.Controllers
{
    [Trait("Contact","")]
    public class ContactControllerTest
    {
        [Fact(DisplayName = "Add action should create contact with required fields")]
        public async Task ContactAddShouldCreateWithRequiredFields()
        {
            var mock = new Mock<IContactService>();
            var contactController = new ContactController(mock.Object);
            
           AutoMapper.Mapper.AddProfile(new ContactViewModelToContact());
            
            var contactViewModel = new ContactAddEditViewModel{FirstName = "Test",Mobile = "012233555"};
            var validationContext = new ValidationContext(contactViewModel, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(contactViewModel, validationContext, validationResults);

            foreach (var validationResult in validationResults)
            {
                contactController.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
           var result = await contactController.Create(contactViewModel) as RedirectToRouteResult;
            
            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
        
        }

        [Fact(DisplayName = "Index action should render contact list")]
        public async Task ContactIndexShouldReturnListOfContact()
        {
            var listOfContacts = new List<ContactListViewModel>();
            listOfContacts.Add(new ContactListViewModel());
            listOfContacts.Add(new ContactListViewModel() );
            var mock = new Mock<IContactService>();
            mock.Setup(m => m.GetAllContactsAsyn(string.Empty,string.Empty)).ReturnsAsync(listOfContacts);

            var contactController = new ContactController(mock.Object);
            var result = await contactController.Index(string.Empty,string.Empty) as ViewResult;
            var model = result.ViewData.Model as List<ContactListViewModel>;
            Assert.Equal(listOfContacts.Count,model.Count);
        }

        [Fact(DisplayName = "Index action should not render Img element when photo path is Null")]
        public async Task ContactIndexShouldNotRenderImgTagWhenPhotoPathIsNull()
        {
            var listOfContacts = new List<ContactListViewModel>();
            listOfContacts.Add(new ContactListViewModel());


            var result = new Views.Contact.Index();

            HtmlDocument resultInHtmlDocument = result.RenderAsHtml(listOfContacts);
            HtmlNode imgNode = resultInHtmlDocument.DocumentNode.SelectSingleNode("//img[@src]");
            Assert.Null( imgNode);

        }

        [Fact(DisplayName = "Edit should update particular contact with minum required feilds")]
        public async Task ContactEditShouldUpdateParticularContactWithMinimumRequiredFields()
        {
            var mock = new Mock<IContactService>();
            
        }

    }
}
