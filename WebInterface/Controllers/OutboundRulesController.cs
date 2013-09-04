﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebInterface.Models;
using MantaMTA.Core.DAL;
using MantaMTA.Core.OutboundRules;
using WebInterfaceLib;
using MantaMTA.Core.MtaIpAddress;

namespace WebInterface.Controllers
{
    public class OutboundRulesController : Controller
    {
        //
        // GET: /OutboundRules/
        public ActionResult Index()
        {
			return View(OutboundRuleDB.GetOutboundRulePatterns());
        }

		//
		// GET: /OutboundRules/Edit?id=
		public ActionResult Edit(int id)
		{
			OutboundMxPattern pattern = OutboundRuleDB.GetOutboundRulePatterns().Single(p=>p.ID == id);
			OutboundRuleCollection rules = new MantaMTA.Core.OutboundRules.OutboundRuleCollection(OutboundRuleDB.GetOutboundRules().Where(r => r.OutboundMxPatternID == id).ToArray());
			MtaIpAddressCollection vMtas = MantaMTA.Core.DAL.MtaIpAddressDB.GetMtaIpAddresses();
			return View(new OutboundRuleModel(rules, pattern, vMtas));
		}

		//
		// GET: /OutboundRules/Delete?patternID=
		public ActionResult Delete(int patternID)
		{
			OutboundRuleWebManager.Delete(patternID);
			return View();
		}
    }
}
