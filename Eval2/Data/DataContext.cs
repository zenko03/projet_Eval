using Eval2.Models;
using Eval2.Models.Import;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Eval2.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        }

       


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                  .UseLowerCaseNamingConvention();
        }
        public DbSet<Coureurs> coureur => Set<Coureurs>();
        public DbSet<Etape> etape => Set<Etape>();
        public DbSet<Equipe> equipe => Set<Equipe>();
        public DbSet<ResultatEtape> resultat => Set<ResultatEtape>();
        public DbSet<Categorie> categorie => Set<Categorie>();
        public DbSet<Course> course => Set<Course>();
        public DbSet<CoureurEtape> coureurEtapes => Set<CoureurEtape>();
        public DbSet<CategorieCoureur> categorieCoureurs => Set<CategorieCoureur>();
        public DbSet<ClassementEtape> ClassementEtapes => Set<ClassementEtape>();
        public DbSet<PointEtape> pointEtapes => Set<PointEtape>();
        public DbSet<Admin> admin => Set<Admin>();
        //CSV

        public DbSet<CsvEtapes> csvEtapes => Set<CsvEtapes>();
        public DbSet<CsvResultat> csvResultats => Set<CsvResultat>();
        public DbSet<CsvPoints> csvPoints => Set<CsvPoints>();



        // VIEW
        public DbSet<V_ResultatEtapeClassement> resultatEtapeClassements { get; set; }
        public DbSet<V_ResultatEtapeClassement_Genre> resultatEtapeClassementsGenre { get; set; }
        public DbSet<V_classement> classements { get; set; }
        public DbSet<V_ClassementCategorie> classementsCategorie { get; set; }
        public DbSet<V_classementequipe> classementsEquipe { get; set; }
        public DbSet<Penalities> penalities { get; set; }





        ////////////////////////

        public DbSet<V_CoureurPoints> coureurPoints { get; set; }
        public DbSet<V_CoureurPoints_Genre> coureurPointsGenre { get; set; }
        public DbSet<EquipesPointsGenre> equipePointsGenre => Set<EquipesPointsGenre>();

        public DbSet<EquipePoints> equipePoints { get; set; }


        //public List<string> ListAllTables()
        //{
        //    var tables = Model.GetEntityTypes()
        //                      .Select(t => t.GetTableName())
        //                      .ToList();
        //    List<string> allTables = new List<string>();
        //    foreach (var table in tables)
        //    {
        //        if (!table.ToLower().Equals("Admin")) allTables.Add(table);
        //    }
        //    return allTables;
        //}





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Categorie>().ToTable("categorie");
            modelBuilder.Entity<Course>().ToTable("course");
            modelBuilder.Entity<Etape>().ToTable("etape");
            modelBuilder.Entity<ResultatEtape>().ToTable("resultat");
            modelBuilder.Entity<Coureurs>().ToTable("coureur");
            modelBuilder.Entity<Equipe>().ToTable("equipe");
            modelBuilder.Entity<CategorieCoureur>().ToTable("categoriecoureur");
            modelBuilder.Entity<CoureurEtape>().ToTable("coureuretape");
            modelBuilder.Entity<ClassementEtape>().ToTable("classementetape");
            modelBuilder.Entity<PointEtape>().ToTable("pointetape");
            modelBuilder.Entity<Admin>().ToTable("admin");
            modelBuilder.Entity<EquipePoints>().ToTable("equipepoints");
            modelBuilder.Entity<EquipesPointsGenre>().ToTable("equipespointsgenre");
            modelBuilder.Entity<Penalities>().ToTable("penalities");


            //CSV
            modelBuilder.Entity<CsvEtapes>().ToTable("csvetape");
            modelBuilder.Entity<CsvResultat>().ToTable("csvresultat");
            modelBuilder.Entity<CsvPoints>().ToTable("Csvpoints");




            // VIEW
            modelBuilder.Entity<V_ResultatEtapeClassement>().HasNoKey().ToView("v_resultatetapeclassement");
            modelBuilder.Entity<V_CoureurPoints>().HasNoKey().ToView("v_coureurpoints");
            modelBuilder.Entity<V_CoureurPoints_Genre>().HasNoKey().ToView("v_coureurpoints_genre");
            modelBuilder.Entity<V_ResultatEtapeClassement_Genre>().HasNoKey().ToView("v_resultatetapeclassement_genre");
            modelBuilder.Entity<V_classementequipe>().HasNoKey().ToView("v_classementequipe");
            modelBuilder.Entity<V_ClassementCategorie>().HasNoKey().ToView("v_classementcategorie");
            modelBuilder.Entity<V_classement>().HasNoKey().ToView("v_classement");












            //  modelBuilder.Entity<ImportCsv>().ToTable("importcsv");

            //modelBuilder.Entity<V_billetVendu>().ToTable(nameof(v_BilletVendus), t => t.ExcludeFromMigrations()); // tsy mirarah table rehefa ampidirina ny migrations
        }

        internal IEnumerable<object> GetSchemaTables()
        {
            throw new NotImplementedException();
        }
    }
}
