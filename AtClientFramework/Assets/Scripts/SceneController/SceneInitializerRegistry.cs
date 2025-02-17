using System.Collections.Generic;

public static class SceneInitializerRegistry
{
    // 단일 씬 초기화 컴포넌트를 관리하기 위한 static 변수
    public static ISceneInitializer CurrentInitializer { get; set; }

    // 등록 시 단순히 CurrentInitializer에 할당
    public static void Register(ISceneInitializer initializer)
    {
        CurrentInitializer = initializer;
    }

    // 등록된 컴포넌트와 일치할 경우 해제
    public static void Unregister(ISceneInitializer initializer)
    {
        if (CurrentInitializer == initializer)
        {
            CurrentInitializer = null;
        }
    }

    // 현재 등록된 초기화 컴포넌트를 T로 캐스팅하여 반환
    public static T GetInitializer<T>() where T : class, ISceneInitializer
    {
        return CurrentInitializer as T;
    }
}