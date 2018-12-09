using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ActionResultSample.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ActionResultSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult ViewResult()
        {
            return View();
        }

        public ActionResult PartialViewResult()
        {

            return PartialView();
        }

        public ActionResult ContenResult()
        {
            var str = @"
<p>直接顯示文字，請看程式碼的註解</p>
<pre>
                return Content(string content) //<span style='color:red;'>方法 1 </span>
                return Content(string content, string contentType) //方法 2 
                return Content(string content, string contentType, Encoding contentEncoding) // 方法 3 
                          </pre>";
            //application/json
            //text/javascript
            //text/xml
            return Content(str);
        }

        public FileContentResult FileContentResult()
        {
            // FileContentResult
            // File(byte[] fileContents, string contentType) 
            // File(byte[] fileContents, string contentType, string fileDownloadName) 

            var db = new NorthwindEntities();
            var category = db.Categories.FirstOrDefault();
            
            //if (category == null || category.Picture == null)
            if (category?.Picture == null)
            {
                return null;
            }

            byte[] image = category.Picture;
            MemoryStream ms = new MemoryStream();
            ms.Write(image, 78, image.Length - 78);
            //return File(ms.ToArray(), "image/jpeg");
            return File(ms.ToArray(), "image/jpeg", "category.jpg");
        }
        public ActionResult FileStreamResult()
        {
            // FileStreamResult
            // File(Stream fileStream, string contentType)
            // File(Stream fileStream, string contentType, string fileDownloadName) 

            var img = new WebImage("~/Content/test.png");

            //這段CODE一點意義都沒有，單純展示可以丟 Stream
            Stream stream = new MemoryStream(img.GetBytes());

            //return File(stream, "application/octet-stream");
            return File(stream, "application/octet-stream", "9981.png");
        }


        public ActionResult FilePathResult()
        {
            // FilePathResult
            // File(string fileName, string contentType)
            // File(string fileName, string contentType, string fileDownloadName) 

            //權限判斷     
            //if (HasPermission(filePath,userId))
            //{

            //}

            var path = this.Server.MapPath("~/Content/test.zip");
            //return File(path, "application/octet-stream");
            return File(path, "application/octet-stream", "請看程式註解.zip");
        }



        public ActionResult EmptyResult()
        {

            return new EmptyResult();
        }


        public ActionResult HttpStatusCodeResult()
        {
            // IIS 7.0、IIS 7.5，及 IIS 8.0 的 HTTP 狀態碼
            // http://support.microsoft.com/kb/943891

            //return new HttpStatusCodeResult(403);
            return new HttpStatusCodeResult(HttpStatusCode.GatewayTimeout, "GatewayTimeout");
        }

        public ActionResult JavaScript()
        {
            return View();
        }

        public ActionResult JavaScriptResult()
        {
            return JavaScript("alert('a')");
        }

        public ActionResult JsonResult()
        {
            /*
             * JsonResult Json(object data) //方法 1 
             * JsonResult Json(object data, string contentType) //方法 2 
             * JsonResult Json(object data, JsonRequestBehavior behavior) //方法 3 
             * JsonResult Json(object data, string contentType, Encoding contentEncoding) //方法 4 
             * JsonResult Json(object data, string contentType, JsonRequestBehavior behavior) //方法 5 
             * JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior) //方法 6 
             */
            var fakeJson = new { name = "SkillTree", MeMber = 3 };

            return Json(fakeJson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PostJsonResult()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PostJsonValue1()
        {
            var fakeJson = new { name = "SkillTree", MeMber = 3 };
            return Json(fakeJson);
        }

        [HttpPost]
        public ActionResult PostJsonValue2()
        {
            var db = new NorthwindEntities();
            var customers = db.Customers.OrderBy(x => x.CustomerID);
            JArray ja = new JArray();
            foreach (var customer in customers)
            {
                JObject jo = new JObject();
                jo.Add("CustomerID", customer.CustomerID);
                jo.Add("CompanyName", customer.CompanyName);
                jo.Add("ContactName", customer.ContactName);
                ja.Add(jo);
            }
            return Content(JsonConvert.SerializeObject(ja), "application/json");
        }

        public ActionResult RedirectResult()
        {
            // MSDN - Controller.Redirect 方法
            // http://msdn.microsoft.com/zh-tw/library/system.web.mvc.controller.redirect(v=vs.118).aspx
            // MSDN - RedirectResult 類別
            // http://msdn.microsoft.com/zh-tw/library/system.web.mvc.redirectresult(v=vs.118).aspx

            //302
            return Redirect("https://dotblogs.com.tw");

            //301
            // return RedirectPermanent("https://dotblogs.com.tw"); 

        }

        public ActionResult RedirectToRouteResult()
        {
            // MSDN - Controller.RedirectToRoute 方法 (Object)
            // http://msdn.microsoft.com/zh-tw/library/dd470174(v=vs.118).aspx
            // MSDN - RedirectToRouteResult 類別
            // http://msdn.microsoft.com/zh-tw/library/system.web.mvc.redirecttorouteresult(v=vs.118).aspx

            return RedirectToRoute("Other");
        }

        public ActionResult RedirectToActionMethod()
        {
            // MSDN - Controller.RedirectToAction 方法
            // http://msdn.microsoft.com/zh-tw/library/system.web.mvc.controller.redirecttoaction(v=vs.118).aspx
            // 沒有 RedirectToAction 類別，最後是使用 RedirectToRoute

            return RedirectToAction("Index", "Home");
        }

    }
}