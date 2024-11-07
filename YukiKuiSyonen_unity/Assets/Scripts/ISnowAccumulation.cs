interface ISnowAccumulation
{
    /// <summary>
    /// 今まで当たった雪の量を返す関数
    /// </summary>
    /// <returns></returns>
    int AccumulatedSnow();

    /// <summary>
    /// 雪のポイントを増やす
    /// </summary>
    /// <param name="snowPoint"></param>
    void AddSnowPoint(int snowPoint);
}
