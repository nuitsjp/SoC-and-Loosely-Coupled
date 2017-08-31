using System;

namespace HatPepper.Integrations
{
    /// <summary>
    /// 端末の位置情報を取得する
    /// </summary>
    public interface IGeoCoordinator
    {
        /// <summary>
        /// 現在地を取得する
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns>タイムアウト時間を経過しても取得できない場合はnullを返す。</returns>
        Location GetCurrent(TimeSpan timeout);
    }
}