using Schols.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Schols.Controllers
{
    public class AccountController : ApiController
    {
        [Route("api/login2")]
        [HttpPost]
        [HttpGet]
        public IHttpActionResult Login([FromBody]string username)
        {
            //return Unauthorized();
            return Json<string>("nfsdkdfs");
        }
        [Route("api/fakelogin")]
        public IHttpActionResult FakeLogin(UserModel user)
        {
            //UserDatabase udb = new UserDatabase();
            //user = udb.ValidUser(user);
            System.Diagnostics.Debug.WriteLine(user.ToString());
            if (user.username.Equals("uche")) user.accesstoken = UserDatabase.generateTokenNoDB(user);
            else user.accesstoken = "";
            if (user.accesstoken.Equals(""))
                return Unauthorized();
            else
                return Ok(user);
        }

        [Route("api/login")]
        public IHttpActionResult Login(UserModel user)
        {
            UserDatabase udb = new UserDatabase();
            user = udb.ValidUser(user);
            if (user.accesstoken.Equals(""))
                return Unauthorized();
            else
                return Ok(user);
        }
        [Route("api/loginwithtoken")]
        [HttpGet]
        public IHttpActionResult TokenLogin()
        {
            HttpContext httpContext = HttpContext.Current;
            NameValueCollection headerList = httpContext.Request.Headers;
            string authorizationField = headerList.Get("Authorization");
            if (authorizationField != null)
            {
                authorizationField = authorizationField.Replace("Bearer ", "");
                UserDatabase udb = new UserDatabase();
                UserModel user = udb.CheckToken(authorizationField);
                System.Diagnostics.Debug.WriteLine(authorizationField);
                System.Diagnostics.Debug.WriteLine(user.username);
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }

        [Route("api/register")]
        public IHttpActionResult Register(UserModel user)
        {
            UserDatabase udb = new UserDatabase();
            string message = udb.RegisterUser(user);
            if (message.Equals(""))
                return Ok(user);
            else
                return NotFound();
        }
        [Route("api/login2")]
        public IHttpActionResult Login(string username, string password)
        {
            return Unauthorized();
        }
        [Route("api/profile")]
        [HttpGet]
        public IHttpActionResult Profile()
        {
            HttpContext httpContext = HttpContext.Current;
            NameValueCollection headerList = httpContext.Request.Headers;
            string authorizationField = headerList.Get("Authorization");
            authorizationField = authorizationField.Replace("Bearer ", "");
            UserDatabase udb = new UserDatabase();
            UserModel user=udb.CheckToken(authorizationField);
            //return "{Message" + ":" + "You-accessed-this-message-with-authorization" + "}"; return Ok(headers.ToString());
            return Ok(user);
        }
        [Route("api/addfavorite")]
        [HttpPost]
        public IHttpActionResult AddFavorite(Favorite fav)
        {
            HttpContext httpContext = HttpContext.Current;
            NameValueCollection headerList = httpContext.Request.Headers;
            string authorizationField = headerList.Get("Authorization");
            authorizationField = authorizationField.Replace("Bearer ", "");
            UserDatabase udb = new UserDatabase();
            UserModel user = udb.CheckToken(authorizationField);
            string message=udb.AddFavorite(fav, user);
            //return "{Message" + ":" + "You-accessed-this-message-with-authorization" + "}"; return Ok(headers.ToString());
            return Ok(message);
        }
        [Route("api/removefavorite")]
        [HttpPost]
        public IHttpActionResult RemoveFavorite(Favorite fav)
        {
            HttpContext httpContext = HttpContext.Current;
            NameValueCollection headerList = httpContext.Request.Headers;
            string authorizationField = headerList.Get("Authorization");
            authorizationField = authorizationField.Replace("Bearer ", "");
            UserDatabase udb = new UserDatabase();
            UserModel user = udb.CheckToken(authorizationField);
            string message = udb.RemoveFavorite(fav, user);
            //return "{Message" + ":" + "You-accessed-this-message-with-authorization" + "}"; return Ok(headers.ToString());
            return Ok(message);
        }

        [Route("api/favorites")]
        [HttpGet]
        public IHttpActionResult GetFavorites()
        {
            //TODO: move token code to another fxn... repetition 
            HttpContext httpContext = HttpContext.Current;
            NameValueCollection headerList = httpContext.Request.Headers;
            string authorizationField = headerList.Get("Authorization");
            authorizationField = authorizationField.Replace("Bearer ", "");
            UserDatabase udb = new UserDatabase();
            UserModel user = udb.CheckToken(authorizationField);
            DBObject db = new DBObject();
            List<Schols.Models.ScholarshipLink> scholarships;
            scholarships = db.GetFavoriteScholarships(user); 
            return Ok(scholarships);
        }
        [Route("api/applications")]
        [HttpPost]
        public IHttpActionResult GetApplications()
        {
            //TODO: move token code to another fxn... repetition 
            HttpContext httpContext = HttpContext.Current;
            NameValueCollection headerList = httpContext.Request.Headers;
            string authorizationField = headerList.Get("Authorization");
            authorizationField = authorizationField.Replace("Bearer ", "");
            UserDatabase udb = new UserDatabase();
            UserModel user = udb.CheckToken(authorizationField);
            DBObject db = new DBObject();
            List<Schols.Models.ScholarshipApp> applications;
            applications = db.GetApplications();
            return Ok(applications);
        }
        [Route("api/apply")]
        [HttpPost]
        public IHttpActionResult ApplyForScholarship(ScholarshipApp app)
        {
            HttpContext httpContext = HttpContext.Current;
            NameValueCollection headerList = httpContext.Request.Headers;
            string authorizationField = headerList.Get("Authorization");
            authorizationField = authorizationField.Replace("Bearer ", "");
            UserDatabase udb = new UserDatabase();
            UserModel user = udb.CheckToken(authorizationField);
            Message message=new Message();
            message.body = udb.Apply(app, user);
            message.title="Application Message";
            //return "{Message" + ":" + "You-accessed-this-message-with-authorization" + "}"; return Ok(headers.ToString());
            return Json(message);
        }
        [Route("api/getapplication")]
        [HttpPost]
        public IHttpActionResult GetApplication(string fund_acct)
        {
            HttpContext httpContext = HttpContext.Current;
            NameValueCollection headerList = httpContext.Request.Headers;
            string authorizationField = headerList.Get("Authorization");
            authorizationField = authorizationField.Replace("Bearer ", "");
            UserDatabase udb = new UserDatabase();
            UserModel user = udb.CheckToken(authorizationField);
            ScholarshipApp application= udb.GetApplication(fund_acct, user);
            //return "{Message" + ":" + "You-accessed-this-message-with-authorization" + "}"; return Ok(headers.ToString());
            return Json(application);
        }

        [Route("api/fileupload")]
        [HttpPost]
        public IHttpActionResult FileUpload()
        {
            HttpContext httpContext = HttpContext.Current;
            NameValueCollection headerList = httpContext.Request.Headers;
            string authorizationField = headerList.Get("Authorization");
            /*
            authorizationField = authorizationField.Replace("Bearer ", "");
            UserDatabase udb = new UserDatabase();
            UserModel user = udb.CheckToken(authorizationField);
             * */
            string file;
            try
            {
                //if (httpContext.Request.QueryString["upload"] != null) **
                {
                    string pathrefer = httpContext.Request.UrlReferrer.ToString();
                    string Serverpath = httpContext.Server.MapPath("Upload");

                    var postedFile = httpContext.Request.Files[0];
                    //For IE to get file name
                    if (httpContext.Request.Browser.Browser.ToUpper() == "IE")
                    {
                        string[] files = postedFile.FileName.Split(new char[] { '\\' });
                        file = files[files.Length - 1];
                    }
                    else
                    {
                        file = postedFile.FileName;
                    }
                    if (!Directory.Exists(Serverpath))
                        Directory.CreateDirectory(Serverpath);

                    string fileDirectory = Serverpath;
                    if (httpContext.Request.QueryString["fileName"] != null)
                    {
                        file = httpContext.Request.QueryString["fileName"];
                        if (File.Exists(fileDirectory + "\\" + file))
                        {
                            File.Delete(fileDirectory + "\\" + file);
                        }
                    }

                    string ext = Path.GetExtension(fileDirectory + "\\" + file);
                    file = Guid.NewGuid() + ext;

                    fileDirectory = Serverpath + "\\" + file;

                    postedFile.SaveAs(fileDirectory);
                /*
                    httpContext.Response.AddHeader("Vary", "Accept");
                    try
                    {
                        if (httpContext.Request["HTTP_ACCEPT"].Contains("application/json"))
                            httpContext.Response.ContentType = "application/json";
                        else
                            httpContext.Response.ContentType = "text/plain";
                    }
                    catch
                    {
                        httpContext.Response.ContentType = "text/plain";
                    }

                    httpContext.Response.Write("Success");
                 * */
                }
            }
            catch (Exception exp)
            {
                return NotFound(); //httpContext.Response.Write(exp.Message);
            }

            //string message = udb.Apply(app, user);
            //return "{Message" + ":" + "You-accessed-this-message-with-authorization" + "}"; return Ok(headers.ToString());
            return Ok(file);
        }
    }
}
