using Cysharp.Threading.Tasks;
using System;

public interface ISceneInitializer
{
    /// <summary>
    /// 씬 내부 초기화를 진행하고, 진행 상황을 progress 리포터에 전달합니다.
    /// </summary>
    /// <param name="progress">0~1 사이의 진행률</param>
    UniTask InitializeAsync(IProgress<float> progress);
}