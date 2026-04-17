using BlazBones.Components;

namespace BlazBones.Services
{
    public record DiceRecords(
        int[] DiceCount,
        int[] TotalCount,
        int[] LockedTotals,
        int SelectedCount,
        int LockedCount,
        int UnselectedCount
    );

    public class DiceCounter
    {
        public DiceCounter() { }
        public DiceRecords Count(Die[] dice) {
            int[] diceCount = { 0, 0, 0, 0, 0, 0 };
            int[] totalCount = { 0, 0, 0, 0, 0, 0 };
            int[] lockedTotals = { 0, 0, 0, 0, 0, 0 };
            int selectedCount = 0;
            int lockedCount = 0;
            int unselectedCount = 6;

            for (int i = 0; i < dice.Length; i++) {
                if (dice[i].locked) {
                    lockedCount++;
                    lockedTotals[dice[i].value - 1]++;
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
            return new DiceRecords(diceCount, totalCount, lockedTotals, selectedCount, lockedCount, unselectedCount);
        }
    }
}
