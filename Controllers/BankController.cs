using MeraBank.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using MeraBank.Models.DL;
namespace MeraBank.Controllers
{
    public class BankController : Controller
    {
        // GET: Bank
        Bank Bnk = new Bank();
        public ActionResult Account()
        {
            return View();
        }
        public ActionResult credit()
        {
            return View();
        }
        public ActionResult ViewDetail()
        {
            return View();
        }
        public ActionResult AccountHolderDetail()
        {
            return PartialView("AccountHolderDetail", Bnk.tbl_AccountHolder.ToList<tbl_AccountHolder>());
        }

        //SAVE Actions
        public ActionResult Save(tbl_AccountHolder Ah)
        {
            if (Ah.ActId == 0 || Ah.ActId == null)
            {
                Bnk.tbl_AccountHolder.Add(Ah);
                Bnk.SaveChanges();
                DataTable dt = Helper.ExecuteTable("SELECT TOP(1) ActId from tbl_AccountHolder order by ActId DESC ");
                Helper.ExecuteQuery("insert into tbl_AccountBalance values('" + Convert.ToInt32(dt.Rows[0][0]) + "','" + 0 + "')");
                Helper.ExecuteQuery("insert into tbl_TranferDetails values('" + Convert.ToInt32(dt.Rows[0][0]) + "','credit','" + DateTime.Now.ToString("dd/MM/yyyy") + "','" + 0 + "','" + 0 + "','" + 0 + "')");

            }
            else
            {
                Bnk.Entry(Ah).State = System.Data.Entity.EntityState.Modified;
                Bnk.SaveChanges();
            }

            ModelState.Clear();
            return View("Account");
        }

        public ActionResult Delete(int id)
        {

            var res = Bnk.tbl_AccountHolder.Where(x => x.ActId == id).FirstOrDefault();
            Bnk.tbl_AccountHolder.Remove(res);
            Bnk.SaveChanges();
            return View("Account");

        }
        public ActionResult Edit(int id)
        {
            return View("Account", Bnk.tbl_AccountHolder.Where(i => i.ActId == id).FirstOrDefault());
        }
        //credit//debit
        public ActionResult GetRecord(string id)
        {
            M_ActDetail ah = new M_ActDetail();
            DataTable dt = Helper.ExecuteTable("select a.ActId,a.ActName,a.CNIC,b.Balance from tbl_AccountHolder a inner join tbl_AccountBalance b on  a.ActId=b.ActId where a.ActId=" + id + "");
            List<M_ActDetail> list = new List<M_ActDetail>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TempData["Account_ID"] = ah.Account_ID = Convert.ToInt32(dt.Rows[i][0]);
                ah.Name = dt.Rows[i][1].ToString();
                ah.CNIC = dt.Rows[i][2].ToString();
                ah.Balance = Convert.ToDouble(dt.Rows[i][3]);
                list.Add(ah);
            }
            return View("credit", list.FirstOrDefault());
        }

        public ActionResult UpdateCredit(string Bal)
        {
            Helper.ExecuteQuery("update tbl_AccountBalance set Balance=Balance+" + Convert.ToDouble(Bal) + " where ActId=" + Convert.ToInt32(TempData["Account_ID"]) + "");
            DataTable dt = Helper.ExecuteTable("select Balance from tbl_AccountBalance where ActId=" + Convert.ToInt32(TempData["Account_ID"]) + "");
            Helper.ExecuteQuery("insert into tbl_TranferDetails values('" + TempData["Account_ID"] + "','credit','" + DateTime.Now.ToString("dd/MM/yyyy") + "','" + Convert.ToDouble(Bal) + "','" + 0 + "','" + Convert.ToDouble(dt.Rows[0][0]) + "')");
            Record();
            return View("credit");
            
        }

        public ActionResult UpdateDebit(string Bal)
        {
            Helper.ExecuteQuery("update tbl_AccountBalance set Balance=Balance-" + Convert.ToDouble(Bal) + " where ActId=" + Convert.ToInt32(TempData["Account_ID"]) + "");
            DataTable dt = Helper.ExecuteTable("select Balance from tbl_AccountBalance where ActId=" + Convert.ToInt32(TempData["Account_ID"]) + "");
            Helper.ExecuteQuery("insert into tbl_TranferDetails values('" + TempData["Account_ID"] + "','Debit','" + DateTime.Now.ToString("dd/MM/yyyy") + "','" +0+ "','" + Convert.ToDouble(Bal) + "','" + Convert.ToDouble(dt.Rows[0][0]) + "')");
            Record();
            return View("credit");
        }

        public ActionResult Record()
        {
            M_ActDetail ah = new M_ActDetail();
            DataTable dt = Helper.ExecuteTable("select a.ActId,a.ActName,a.CNIC,b.Balance from tbl_AccountHolder a inner join tbl_AccountBalance b on  a.ActId=b.ActId where a.ActId=" + TempData["Account_ID"] + "");
            List<M_ActDetail> list = new List<M_ActDetail>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TempData["Account_ID"] = ah.Account_ID = Convert.ToInt32(dt.Rows[i][0]);
                ah.Name = dt.Rows[i][1].ToString();
                ah.CNIC = dt.Rows[i][2].ToString();
                ah.Balance = Convert.ToDouble(dt.Rows[i][3]);
                list.Add(ah);
            }
            return View(list.FirstOrDefault());
        }
        public ActionResult ViewTDetail(string id)
        {
            
            DataTable dt = Helper.ExecuteTable("select*from tbl_TranferDetails where ActId="+Convert.ToInt32(id)+"");
            List<tbl_TranferDetails> list = new List<tbl_TranferDetails>();
         
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tbl_TranferDetails T = new tbl_TranferDetails();
                T.ActId = Convert.ToInt32(dt.Rows[i][1]);
                T.TrsCategory = dt.Rows[i][2].ToString();
                T.TrsDate = dt.Rows[i][3].ToString(); 
                T.Credit = Convert.ToDecimal(dt.Rows[i][4]);
                T.Debit = Convert.ToDecimal(dt.Rows[i][5]);
                T.Balance = Convert.ToDecimal(dt.Rows[i][6]);
                list.Add(T);
            }
            return View("ViewDetail",list);
          
        }

    }
}