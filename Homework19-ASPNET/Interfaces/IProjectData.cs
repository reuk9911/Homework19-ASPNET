using NuGet.Protocol.Core.Types;

namespace Homework19_ASPNET.Interfaces
{
    public interface IProjectData
    {
        IEnumerable<Project> GetProjects();
        Project GetProjectById(int id);
        void AddProject(Project project);

        void RemoveProject(int id);


    }
}
