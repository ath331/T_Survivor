using Unity.Jobs;

public class Job : IJob
{
    private JobData _data;

    public Job(JobData data)
    {
        _data = data;
    }

    public JobType JobType => (JobType)_data.jobType;
    public string JobName => _data.jobType.ToString();

    public bool CanEquip(WeaponType weaponType)
    {
        // TODO: JSON 데이터에 기반한 로직으로 수정
        return true;
    }
}