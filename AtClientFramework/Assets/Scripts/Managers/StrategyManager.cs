using System.Collections.Generic;
using System.Linq;

public class StrategyManager
{
    private List<IStrategy> strategies = new List<IStrategy>();

    public void RegisterAllStrategies()
    {
        // IStrategy 인터페이스를 구현한 전략을 모두 등록
        strategies.Add(new Chat_Strategy());
        strategies.Add(new Login_Strategy());
        strategies.Add(new EnterGame_Strategy());
        strategies.Add(new EnterLobby_Strategy());
        strategies.Add(new Move_Strategy());
        strategies.Add(new Animation_Strategy());
        strategies.Add(new Spawn_Strategy());
        strategies.Add(new RoomCreate_Strategy());
        strategies.Add(new RequestAllRoom_Strategy());
    }

    // 필요 시 특정 전략 접근 및 관리 기능 추가
    public T GetStrategy<T>() where T : class, IStrategy
    {
        return strategies.FirstOrDefault(s => s is T) as T;
    }
}
