using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext context)
        {
            context.Database.Migrate();

            // Õlud
            for (int i = context.Olud.Count(); i < 10; i++)
            {
                context.Olud.Add(new Olu
                {
                    Nimi = $"Katseõlu {i + 1}",
                    Kirjeldus = $"Katseõlle {i + 1} kirjeldus",
                    Tuup = i % 2 == 0 ? "Lager" : "Porter",
                    Alkoholiprotsent = 4.5m + i * 0.2m
                });
            }

            context.SaveChanges();

            var olud = context.Olud
                .OrderBy(x => x.Id)
                .Take(10)
                .ToList();

            // Partiide loomine
            for (int i = context.Partiid.Count(); i < 10; i++)
            {
                context.Partiid.Add(new Partii
                {
                    OluId = olud[i % olud.Count].Id,
                    Kood = $"SEED-{i + 1:000}",
                    Kuupaev = new DateTime(2026, 1, 1).AddDays(i),
                    Kirjeldus = $"Proovipartii {i + 1}",
                    Tulemus = "Partii on valmis ja proovitud."
                });
            }

            context.SaveChanges();

            var partiid = context.Partiid
                .OrderBy(x => x.Id)
                .Take(10)
                .ToList();

            // Koostisosad
            for (int i = context.Koostisosad.Count(); i < 10; i++)
            {
                context.Koostisosad.Add(new Koostisosa
                {
                    PartiiId = partiid[i % partiid.Count].Id,
                    Nimetus = $"Linnased {i + 1}",
                    Uhik = "kg",
                    Hind = 2.50m + i * 0.10m,
                    Kogus = 4m + i * 0.5m,
                    Kirjeldus = "Proovipartii koostisosa"
                });
            }

            // Maitsmised
            for (int i = context.Maitsmised.Count(); i < 10; i++)
            {
                context.Maitsmised.Add(new Maitsmine
                {
                    PartiiId = partiid[i % partiid.Count].Id,
                    Kuupaev = new DateTime(2026, 2, 1).AddDays(i),
                    Degusteerija = $"Testkasutaja {i + 1}",
                    Hinne = 6 + i % 5,
                    Kommentaar = $"Proovipartii {i + 1} hinnang"
                });
            }

            // Pruulimislogid
            for (int i = context.PruulimisLogid.Count(); i < 10; i++)
            {
                context.PruulimisLogid.Add(new PruulimisLogi
                {
                    PartiiId = partiid[i % partiid.Count].Id,
                    Kuupaev = new DateTime(2026, 1, 15).AddDays(i),
                    Kasutaja = $"Testkasutaja {i + 1}",
                    Kirjeldus = "Kontrolliti käärimist ja temperatuuri."
                });
            }

            // Fotode testkirjed; failid lisatakse eraldi.
            for (int i = context.PartiiFotod.Count(); i < 10; i++)
            {
                context.PartiiFotod.Add(new PartiiFoto
                {
                    PartiiId = partiid[i % partiid.Count].Id,
                    FailiTee = $"images/partiid/testfoto-{i + 1:000}.jpg"
                });
            }

            context.SaveChanges();
        }
    }
}