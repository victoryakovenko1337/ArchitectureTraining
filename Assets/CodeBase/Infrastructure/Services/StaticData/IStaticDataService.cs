using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        MonsterStaticData ForMonster(MonsterTypeId typeId);
        LevelStaticData ForLevel(string sceneKey);
        WindowConfig ForWindow(WindowId shop);
        void Load();
    }
}