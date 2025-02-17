using Cysharp.Threading.Tasks;
using System;

public interface ISceneInitializer
{
    /// <summary>
    /// �� ���� �ʱ�ȭ�� �����ϰ�, ���� ��Ȳ�� progress �����Ϳ� �����մϴ�.
    /// </summary>
    /// <param name="progress">0~1 ������ �����</param>
    UniTask InitializeAsync(IProgress<float> progress);
}