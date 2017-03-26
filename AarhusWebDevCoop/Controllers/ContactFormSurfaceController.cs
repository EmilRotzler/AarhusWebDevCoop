using AarhusWebDevCoop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Umbraco.Core.Models;


namespace AarhusWebDevCoop.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: ContactFormSurface
        public ActionResult Index()
        {
            return PartialView("ContactForm", new ContactForm());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid) { return CurrentUmbracoPage(); }
            MailMessage message = new MailMessage();
            message.To.Add("emilarotzler@hotmail.com");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message;

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("evanbreener@gmail.com", "evanbreen1234");
                smtp.EnableSsl = true;
                //send mail
                smtp.Send(message);
            }            IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "message");            comment.SetValue("messageName", model.Name);            comment.SetValue("email", model.Email);            comment.SetValue("subject", model.Subject);            comment.SetValue("messageContent", model.Message);

            //Save
            Services.ContentService.Save(comment);            //Save and publish
            //Services.ContentService.SaveAndPublishWithStatus(comment);
            TempData["success"] = true;
            return RedirectToCurrentUmbracoPage();
        }
    }
}