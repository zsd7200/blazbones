using BlazBones.Components;

public record DiceRecords(
    int[] DiceCount,
    int[] TotalCount,
    int SelectedCount,
    int LockedCount,
    int UnselectedCount
);

public class DiceCounter
{
    public DiceCounter() { }
    public DiceRecords Count(Die[] dice)
    {
        int[] diceCount = { 0, 0, 0, 0, 0, 0 };
        int[] totalCount = { 0, 0, 0, 0, 0, 0 };
        int selectedCount = 0;
        int lockedCount = 0;
        int unselectedCount = 6;

        for (int i = 0; i < dice.Length; i++) {
            if (dice[i].locked) {
                lockedCount++;
                continue;
            }

            // ignore locked when counting total
            totalCount[dice[i].value - 1]++;

            if (dice[i].selected) {
                selectedCount++;
                diceCount[dice[i].value - 1]++;
                continue;
            }
        }

        unselectedCount -= lockedCount + selectedCount;
        Console.WriteLine("------------");
        for (int i = 0; i < totalCount.Length; i++)
        {
            Console.WriteLine(i + ": " + totalCount[i]);
        }
        return new DiceRecords(diceCount, totalCount, selectedCount, lockedCount, unselectedCount);
    }
}