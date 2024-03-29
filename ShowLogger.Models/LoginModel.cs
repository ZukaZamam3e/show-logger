﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Models;
public class LoginModel
{
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Display(Name = "Password")]
    public string Password { get; set; }

    [Display(Name = "Remember Me?")]

    public bool IsPersistent { get; set; }

    public string ReturnUrl { get; set; }
}

public class OARegisterModel
{
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Display(Name = "Email")]
    public string Email { get; set; }

    [Display(Name = "Password")]
    public string Password { get; set; }

    [Display(Name = "Retype Password")]
    public string RetypePassword { get; set; }
}


