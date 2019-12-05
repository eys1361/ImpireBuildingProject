
using ImpireBuilding.Controllers;
using ImpireBuilding.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImoireBuildingTests
{
    public class VisitorsTests
    {
        [Fact]
        public async void Test_Create_Visitors()
        {
            //Arrange           
            var db = MockDb.CreateMockDb();
            var v = new VisitorsController(db);

            var visitor = new Visitor { FirstName = "Alex", LastName = "Coa", PhoneNumber = "6476666888", ParkingId = 5, TimeOfEntry = System.DateTime.Today, TimeOfExit = System.DateTime.Today };

            //Act
            var r = await v.Create(visitor);

            //Assert
            var result = Assert.IsType<RedirectToActionResult>(r);
            Assert.Equal("Index", result.ActionName);
            // Each one can be used
            Assert.Equal(2, db.visitor.Count());
            Assert.Equal(1, db.visitor.Where(x => x.FirstName == visitor.FirstName && x.LastName == visitor.LastName && x.PhoneNumber == visitor.PhoneNumber && x.TimeOfEntry == visitor.TimeOfEntry && x.TimeOfExit == visitor.TimeOfExit).Count());
        }

        [Fact]
        public async void Test_Create_Invalid_Visitor_Name()
        {
            //Arrange
            var db = MockDb.CreateMockDb();
            var c = new VisitorsController(db);

            var visitor = new Visitor { LastName = "Coa", PhoneNumber = "6476666888", ParkingId = 5, TimeOfEntry = System.DateTime.Today, TimeOfExit = System.DateTime.Today };
            c.ModelState.AddModelError("Name", "Required");

            //Act
            var r = await c.Create(visitor);

            //Assert
            var result = Assert.IsType<ViewResult>(r);
            var model = Assert.IsAssignableFrom<Visitor>(result.ViewData.Model);
            Assert.Equal(visitor, model);
            Assert.IsType<SelectList>(result.ViewData["parkingId"]);

        }

        [Fact]
        public async void Test_Create_Invalid_Visitor_TimeIn()
        {
            //Arrange
            var db = MockDb.CreateMockDb();
            var c = new VisitorsController(db);

            var visitor = new Visitor { FirstName = "Alex", LastName = "Coa", PhoneNumber = "6476666888", ParkingId = 5, TimeOfExit = System.DateTime.Today };
            c.ModelState.AddModelError("TimeOfExit", "Required");

            //Act
            var r = await c.Create(visitor);

            //Assert
            var result = Assert.IsType<ViewResult>(r);
            var model = Assert.IsAssignableFrom<Visitor>(result.ViewData.Model);
            Assert.Equal(visitor, model);
            Assert.IsType<SelectList>(result.ViewData["parkingId"]);
        }

        [Fact]
        public async void Test_Create_Invalid_Visitor_TimeOut()
        {
            //Arrange
            var db = MockDb.CreateMockDb();
            var c = new VisitorsController(db);

            var visitor = new Visitor { FirstName = "Alex", LastName = "Coa", PhoneNumber = "6476666888", ParkingId = 5, TimeOfEntry = System.DateTime.Today };
            c.ModelState.AddModelError("Description", "MaxWords(4)");

            //Act
            var r = await c.Create(visitor);
            //Assert
            var result = Assert.IsType<ViewResult>(r);
            var model = Assert.IsAssignableFrom<Visitor>(result.ViewData.Model);
            Assert.Equal(visitor, model);
            Assert.IsType<SelectList>(result.ViewData["parkingId"]);
        }

        [Fact]
        public async void IndexTest()
        {
            //Arrange
            var dbContext = MockDb.CreateMockDb();
            var sc = new VisitorsController(dbContext);
            //Act
            var r = await sc.Index();
            //Assert
            var result = Assert.IsType<ViewResult>(r);
            var model = Assert.IsAssignableFrom<List<Visitor>>(result.ViewData.Model);
            Assert.Single(model);
        }

        [Fact]
        public async void Test_Delete_Visitor()
        {
            //Arrange           
            var db = MockDb.CreateMockDb();
            var c = new VisitorsController(db);
            var visitor = new Visitor { FirstName = "Alex", LastName = "Coa", PhoneNumber = "6476666888", ParkingId = 5, TimeOfEntry = System.DateTime.Today, TimeOfExit = System.DateTime.Today };
            //Act
            var r = await c.Delete(1);
            //Assert
            var result = Assert.IsType<ViewResult>(r);
            var model = Assert.IsAssignableFrom<Visitor>(result.Model);

            Assert.Equal(visitor, model);
        }


    }
}
