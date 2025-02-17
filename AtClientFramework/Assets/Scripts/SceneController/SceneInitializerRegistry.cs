using System.Collections.Generic;

public static class SceneInitializerRegistry
{
    // ���� �� �ʱ�ȭ ������Ʈ�� �����ϱ� ���� static ����
    public static ISceneInitializer CurrentInitializer { get; set; }

    // ��� �� �ܼ��� CurrentInitializer�� �Ҵ�
    public static void Register(ISceneInitializer initializer)
    {
        CurrentInitializer = initializer;
    }

    // ��ϵ� ������Ʈ�� ��ġ�� ��� ����
    public static void Unregister(ISceneInitializer initializer)
    {
        if (CurrentInitializer == initializer)
        {
            CurrentInitializer = null;
        }
    }

    // ���� ��ϵ� �ʱ�ȭ ������Ʈ�� T�� ĳ�����Ͽ� ��ȯ
    public static T GetInitializer<T>() where T : class, ISceneInitializer
    {
        return CurrentInitializer as T;
    }
}