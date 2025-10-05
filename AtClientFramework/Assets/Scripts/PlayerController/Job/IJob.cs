public interface IJob
{
    string JobName { get; }
    bool CanEquip(WeaponType weaponType);
}