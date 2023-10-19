
[System.Serializable]
public class Tutorial
{
    public string name;
    public int showAtThisIndex;
    public TutorialType tutorialType;
}

public enum TutorialType
{
    LearnCannonBall,
    LearnAwardBox,
    LearnTNT,
    FruitPointer
}
