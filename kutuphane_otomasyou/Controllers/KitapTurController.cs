using kutuphane_otomasyou.Models.table.kitaplar;
using kutuphane_otomasyou.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kutuphane_otomasyou.Models.table;

namespace kutuphane_otomasyou.Controllers
{
    public class KitapTurController : Controller
    {
        [Authorize]
        public ActionResult kitapTuruEkle(string tur_adi)
        {
            if (Session["gizli"] != null)
            {
                databaseContextcs db = new databaseContextcs();

                if (!string.IsNullOrEmpty(tur_adi))
                {
                    KitapTuru yeni_tur = new KitapTuru
                    {
                        tur_adi = tur_adi,
                    };

                    db.kitapTuru.Add(yeni_tur);
                    db.SaveChanges();

                    return RedirectToAction("TurSil", "KitapTur"); // Metodu tamamlayan dönüş ifadesi

                }
                else
                {
                    return View(); // Metodu tamamlayan dönüş ifadesi
                }
                return View();
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
        }

        [Authorize]
        public ActionResult TurSil(int? TurId)
        {
            if (Session["gizli"] != null)
            {
                databaseContextcs db = new databaseContextcs();
                List<KitapTuru> turTablosu = db.kitapTuru.ToList();
                if (TurId.HasValue && TurId > 0)
                {
                    KitapTuru turSil = db.kitapTuru.Where(x => x.Id == TurId).FirstOrDefault();
                    List<AlinanKitaplar> alinanKitaplar = db.AlinanKitapTaplosu.Where(x => x.turuId == TurId).ToList();
                    if (alinanKitaplar != null && alinanKitaplar.Count < 1)
                    {
                        foreach (var item in alinanKitaplar)
                        {
                            db.AlinanKitapTaplosu.Remove(item);
                        }
                        List<Kitap> kitaplar = db.kitaptablosu.Where((x) => x.turuId == TurId).ToList();
                        foreach (var item in kitaplar)
                        {
                            db.kitaptablosu.Remove(item);
                        }
                        db.kitapTuru.Remove(turSil);
                        db.SaveChanges();
                        turTablosu = db.kitapTuru.ToList();
                    }
                    else
                    {
                        ViewBag.hata = "Bu türde düzenleme yapamazsınız !";
                    }


                }
                return View(turTablosu);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }

        }

        [Authorize, HttpGet]
        public ActionResult TurDuzenle(int? turId)
        {
            if (Session["gizli"] != null)
            {
                databaseContextcs db = new databaseContextcs();
                KitapTuru tur = null;

                if (turId != null)
                {
                    tur = db.kitapTuru.Where(x => x.Id == turId).FirstOrDefault();
                }

                return View(tur);
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
        }

        [Authorize, HttpPost]
        public ActionResult TurDuzenle(KitapTuru model, int? turId)
        {
            if (Session["gizli"] != null)
            {
                databaseContextcs db = new databaseContextcs();
                KitapTuru KitapTur = null;

                if (turId != null && model != null)
                {
                    KitapTur = db.kitapTuru.Where(x => x.Id == turId).FirstOrDefault();

                    KitapTur.tur_adi = model.tur_adi;
                    int result = db.SaveChanges();

                    if (result > 0)
                    {

                    }
                    else
                    {
                        TempData["bos"] = "bos";
                        return View(KitapTur);
                    }
                }
                else
                {
                    TempData["bos"] = "bos";
                    return View(KitapTur);
                }

                return RedirectToAction("TurSil", "KitapTur");
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
        }
    }
}