using Containers.Models;

namespace Containers.Application;

public interface IConrainerService
{
    IEnumerable<Container> GetAllContainers();
    bool AddContainer(Container container);
}