using System.Diagnostics;

namespace Homework19_ASPNET.Data
{
    public static class DbInitializer
    {
        public static void Initialize(Homework19_ASPNETContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Project.Any())
            {
                return;   // DB has been seeded
            }

            var projects = new Project[]
            {
            new Project{ID=1, Name="Василий", StartDate=DateTime.Parse("2005-09-01"), EndDate=DateTime.Parse("2005-09-02")},
            new Project{ID=1, Name="Маруся", StartDate=DateTime.Parse("2005-09-03"), EndDate=DateTime.Parse("2005-09-04")},
            new Project{ID=1, Name="Егор", StartDate=DateTime.Parse("2005-09-05"), EndDate=DateTime.Parse("2005-09-06")},
            };
            foreach (Project s in projects)
            {
                context.Project.Add(s);
            }
            context.SaveChanges();

            
        }
    }
}
