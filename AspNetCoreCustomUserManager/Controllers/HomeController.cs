// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AspNetCoreCustomUserManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreCustomUserManager
{
  public class HomeController : Controller
  {
    private IUserManager _userManager;

    public HomeController(IUserManager userManager)
    {
      _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Index()
    {
      return View();
    }

    [HttpPost]
    public IActionResult Login()
    {
      User user = _userManager.Validate("Email", "admin@example.com", "admin");

      if (user != null)
        _userManager.SignIn(HttpContext, user, false);

      return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Logout()
    {
      _userManager.SignOut(HttpContext);
      return RedirectToAction("Index");
    }
  }
}