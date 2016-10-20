using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCLogin2.Models;
using System.Web.Security;
namespace MVCLogin2.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        ///<summary>
        ///realiza el llamado a la vista que contiene la GUI de autenticacion de la aplicacion
        ///</summary>
        ///<returns></returns>
        ///

        [HttpGet]
        public ActionResult logIn()
        {
            return View();
        }

        ///<summary>
        ///verificar los datos suministrados por el ususario al realizar la peticion post  de envio de información 
        ///mediante la GUI de Autenticación de la application
        ///</summary>
        ///<param name="user"></param>
        ///<returns></returns>
        ///

        [HttpPost]
        public ActionResult logIn(Models.UserModel user)
        {
            if (ModelState.IsValid) //  Verificar que el modelo sea valido en cuanto a la definicion de propiedades
            {
                if (IsValid(user.Email,user.Password))// verificar que el email y clave exista utilizando el metodo privado
                {
                   FormsAuthentication.SetAuthCookie(user.Email,false) ;//Crea la variable usuario con el correo del usuario
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ModelState.AddModelError("","Login Data in Incorrect");//agregar mensaje de error al model
                }

            
            }
            return View(user);
        }

        ///<summary>
        ///realizar el llamado de la vista de Registro
        ///</summary>
        ///<returns></returns>
        ///
        public ActionResult Registration()
        {
            return View();
        }

        /// <summary>
        /// Verificar los datos suministrados por el usuario al realizar la petición Post de envió de información
        /// mediante la GUI para crear un nuevo usuario en el sistema
        /// </summary>
        /// <param name=”user”></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult Registration(Models.UserModel user)
        {
            try
            {
             if (ModelState.IsValid)
            {
                using (var db = new DBMvcLoginEntities()) //crear objeto con referencia a la base de datos para crear el nuevo usuario
                {
                    var  exist= db.Users.Find(user.Email);
                    if (exist!=null)
                    {
                        @ViewBag.message="Usuario ya existe";
                        return Registration();

                    }
                    else
                    {
                        var Sysuser = db.Users.Create();

                        Sysuser.Email = user.Email;
                        Sysuser.Password = user.Password;
                        Sysuser.PasswordSalt = user.Password;
                        Sysuser.UserId = Guid.NewGuid();
                        db.Users.Add(Sysuser);
                        db.SaveChanges();

                        return RedirectToAction("Login", "Login");
                    }
                   
                }
            }
            else
            {
                ModelState.AddModelError("","Login data is incorrect");
            }

            return View();
            }
            catch (Exception ex)
            {
                
              return View(ex.Message);
            }
        }
        /// <summary>
        /// Cerrar sesión del usuario autenticado
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","Login");
        }
        /// <summary>
        /// Metodo para validar el email y password del usuario, realiza la consulta a la base de datos
        /// </summary>
        /// <param name=”Email”>Email ingresado</param>
        /// <param name=”password”>Password ingresado</param>
        /// <returns>
        /// True:Usuario valido
        /// False Usuario Invalido
        /// </returns>
        /// 
        private bool IsValid(string Email, string password)
        {
            bool IsValid=false;
            using(var db = new DBMvcLoginEntities())
            {
                var user=db.Users.FirstOrDefault(u=> u.Email==Email);

                if (user!=null)
                {
                    if (user.Password==password)
                    {
                        IsValid=true;
                    }
                }
            }
            return IsValid;
        }
    }
}