using StreamPartyCommand.Views;
using Zenject;

namespace StreamPartyCommand.Installers
{
    public class SPCMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            _ = this.Container.BindInterfacesAndSelfTo<SettingViewController>().FromNewComponentAsViewController().AsSingle();

        }
    }
}
