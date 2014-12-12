using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;

namespace Schols.Models
{
    public class UserDatabase
    {
        public UserModel ValidUser(UserModel user)
        {
            DBObject db = new DBObject();

            string sqlstr = "SELECT passwordhash,salt,fullname,usermajor FROM users where username= @username";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@username", user.username));
            DataTable dt = db.querySQLServer(sqlstr, parameters);
            if (dt.Rows.Count == 0)
            {
                user.accesstoken = "";
                user.username = "";
                return user;
            }
            string storedHash = dt.Rows[0]["passwordhash"].ToString();
            string storedSalt = dt.Rows[0]["salt"].ToString();
            string fullname = dt.Rows[0]["fullname"].ToString();
            string usermajor = dt.Rows[0]["usermajor"].ToString();
            string inputHash = CreatePasswordHash(user.userpassword, storedSalt);
            if (storedHash.Equals(inputHash))
            {
                user.accesstoken = generateToken(user);
                user.fullname = fullname;
                user.usermajor = usermajor;
                return user;
            }
            else
            {
                user.accesstoken = "";
                user.username = "";
                return user;
            }
        }
        public bool UserExists(UserModel user)
        {
            DBObject db = new DBObject();

            String sqlstr = "SELECT * FROM users where username= @username";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@username", user.username));
            DataTable dt = db.querySQLServer(sqlstr, parameters);
            if (dt.Rows.Count == 0)
                return false;
            else
                return true;
        }
        private UserModel GetUser(UserModel user)
        {
            DBObject db = new DBObject();
            String sqlstr = "SELECT * FROM users where username= @username";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@username", user.username));
            DataTable dt = db.querySQLServer(sqlstr, parameters);
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                user.fullname = dt.Rows[0]["fullname"].ToString();
                user.usermajor = dt.Rows[0]["usermajor"].ToString();
                System.Diagnostics.Debug.WriteLine(user.fullname);
                System.Diagnostics.Debug.WriteLine(user.username);
                return user;
            }
        }

        public string RegisterUser(UserModel user)
        {
            if (UserExists(user))
            {
                return "User Exists Already";
            }
            DBObject db = new DBObject();
            String sqlstr = "INSERT INTO users (username,passwordhash,salt,fullname,usermajor) VALUES (@username, @passwordhash,@salt,@fullname,@usermajor)";
            string salt = CreateSalt(4);
            string passwordhash = CreatePasswordHash(user.userpassword, salt);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@username", user.username));
            parameters.Add(new SqlParameter("@passwordhash", passwordhash));
            parameters.Add(new SqlParameter("@salt", salt));
            parameters.Add(new SqlParameter("@fullname", user.fullname));
            parameters.Add(new SqlParameter("@usermajor", user.usermajor));
            int count = db.queryExecuteSQLServer(sqlstr, parameters);
            if (count == 1)
            {
                return "";
            }
            else
            {
                return "Could not create user.";
            }
        }
        public string Apply(ScholarshipApp app, UserModel user)
        {
            string sqlstr = "SELECT * FROM applications WHERE username=@username AND fund_acct=@fund_acct";
            string message = "";
            DBObject db = new DBObject();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@username", user.username));
            parameters.Add(new SqlParameter("@fund_acct", app.fund_acct));

            DataTable dt = db.querySQLServer(sqlstr, parameters);
            if (dt.Rows.Count == 0)
            {
                sqlstr = "INSERT INTO applications (universityid,firstname,middlename,lastname,address,phonenumber,email,username,fund_acct,essayfilename,reffilename) VALUES (@universityid,@firstname,@middlename,@lastname,@address,@phonenumber,@email,@username,@fund_acct,@essayfilename,@reffilename)";
                List<SqlParameter> insertParameters = new List<SqlParameter>();
                insertParameters.Add(new SqlParameter("@universityid", app.universityid));
                insertParameters.Add(new SqlParameter("@firstname", app.firstname));
                insertParameters.Add(new SqlParameter("@middlename", app.middlename));
                insertParameters.Add(new SqlParameter("@lastname", app.lastname));
                insertParameters.Add(new SqlParameter("@address", app.address));
                insertParameters.Add(new SqlParameter("@phonenumber", app.phonenumber));
                insertParameters.Add(new SqlParameter("@email", app.email));
                insertParameters.Add(new SqlParameter("@username", user.username));
                insertParameters.Add(new SqlParameter("@fund_acct", app.fund_acct));
                insertParameters.Add(new SqlParameter("@essayfilename", app.essayfilename));
                insertParameters.Add(new SqlParameter("@reffilename", app.reffilename));
                int count = db.queryExecuteSQLServer(sqlstr, insertParameters);
                message = "Application Submitted";
            }
            else
            {
                message = "Application exists already";
            }
            return message;
        }
        public string AddFavorite(Favorite fav, UserModel user)
        {
            string sqlstr = "SELECT * FROM favorites WHERE username=@username AND fund_acct=@fundacct";
            string message = "";
            DBObject db = new DBObject();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@username", user.username));
            parameters.Add(new SqlParameter("@fundacct",fav.fundacct));
            
            DataTable dt = db.querySQLServer(sqlstr, parameters);
            if (dt.Rows.Count == 0)
            {
                sqlstr = "INSERT INTO favorites (username,fund_acct,frml_schlrshp_name) VALUES (@username,@fundacct, @schlrshpname)";
                List<SqlParameter> insertParameters = new List<SqlParameter>();
                insertParameters.Add(new SqlParameter("@username", user.username));
                insertParameters.Add(new SqlParameter("@fundacct", fav.fundacct));
                insertParameters.Add(new SqlParameter("@schlrshpname", fav.schlrshpname));
                //same parameter list applies
                int count = db.queryExecuteSQLServer(sqlstr, insertParameters);
                message = "Added to Favorites";
            }
            else
            {
                message = "Already exists in favorites";
            }
            return message;
        }
        public string RemoveFavorite(Favorite fav, UserModel user)
        {
            string sqlstr = "DELETE FROM favorites WHERE username=@username AND fund_acct=@fundacct";
            string message = "";
            DBObject db = new DBObject();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@username", user.username));
            parameters.Add(new SqlParameter("@fundacct", fav.fundacct));

            int count=db.queryExecuteSQLServer(sqlstr, parameters);
            message = "Deleted.";
            return message;
        }

        public string generateToken(UserModel user)
        {
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            DBObject db = new DBObject();
            String sqlstr = "INSERT INTO tokens (username,accesstoken,granted) VALUES (@username, @accesstoken,@granted)";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@username", user.username));
            parameters.Add(new SqlParameter("@accesstoken", token));
            parameters.Add(new SqlParameter("@granted", DateTime.Now));
            int count = db.queryExecuteSQLServer(sqlstr, parameters);
            return token;
        }
        public static string generateTokenNoDB(UserModel user)
        {
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return token;
        }
        public UserModel CheckToken(string token)
        {
            DBObject db = new DBObject();
            string sqlstr = "SELECT username FROM tokens WHERE accesstoken= @accesstoken"; //and granted...
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@accesstoken", token));
            DataTable dt = db.querySQLServer(sqlstr, parameters);
            if (dt.Rows.Count == 0)
                return null;
            else
            {
                UserModel user = new UserModel();
                user.username = dt.Rows[0]["username"].ToString();
                return GetUser(user); // dt.Rows[0]["username"].ToString();
            }

        }
        private string CreateSalt(int size)
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }

        private string CreatePasswordHash(string pwd, string salt)
        {
            string saltAndPwd = String.Concat(pwd, salt);
            byte[] saltAndPwdbytes = System.Text.Encoding.Unicode.GetBytes(saltAndPwd);
            var sha = new SHA1Managed();
            string hashedPwd = Convert.ToBase64String(sha.ComputeHash(Convert.FromBase64String(Convert.ToBase64String(saltAndPwdbytes)))); //FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "sha1");
            return hashedPwd;
        }

        internal ScholarshipApp GetApplication(string fund_acct, UserModel user)
        {
            DBObject db = new DBObject();
            String sqlstr = "SELECT * FROM applications where username= @username and fund_acct=@fund_acct";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@username", user.username));
            parameters.Add(new SqlParameter("@fund_acct", fund_acct));
            DataTable dt = db.querySQLServer(sqlstr, parameters);
            ScholarshipApp application;
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                int i = 0;
                application = new ScholarshipApp();
                application.universityid = dt.Rows[i]["universityid"].ToString().Trim();
                application.firstname = dt.Rows[i]["firstname"].ToString().Trim();
                application.lastname = dt.Rows[i]["lastname"].ToString().Trim();
                application.middlename = dt.Rows[i]["middlename"].ToString().Trim();
                application.address = dt.Rows[i]["address"].ToString().Trim();
                application.phonenumber = dt.Rows[i]["phonenumber"].ToString().Trim();
                application.email = dt.Rows[i]["email"].ToString().Trim();
                application.fund_acct = dt.Rows[i]["fund_acct"].ToString().Trim();
                application.username = dt.Rows[i]["username"].ToString().Trim();
                application.essayfilename = dt.Rows[i]["essayfilename"].ToString().Trim();
                application.reffilename = dt.Rows[i]["reffilename"].ToString().Trim();
                application.scholarshipyear = dt.Rows[i]["scholarshipyear"].ToString().Trim();
                //System.Diagnostics.Debug.WriteLine("Row : " + i.ToString());
                return application;
            }
        }
    }
}