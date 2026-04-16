public class Logic {
    private int[] oneScores = [100, 200, 1000, 2000, 4000, 8000];
    private int[] twoScores = [200, 400, 800, 1600];
    private int[] threeScores = [300, 600, 1200, 2400];
    private int[] fourScores = [400, 800, 1600, 3200];
    private int[] fiveScores = [50, 100, 500, 1000, 2000, 4000];
    private int[] sixScores = [600, 1200, 2400, 4800];
    private List<int[]> scores = [];
    private int straightScore = 1000;
    private int backwardScore = -100;

    public Logic()
    {
        InitializeScores();
    }

    public bool ValidateSelection(int[] diceCount) {
        // 2s, 3s, 4s, and 6s are illegal if only 1 or 2 are selected
        if (diceCount[1] == 1 || diceCount[1] == 2) return false;
        if (diceCount[2] == 1 || diceCount[2] == 2) return false;
        if (diceCount[3] == 1 || diceCount[3] == 2) return false;
        if (diceCount[5] == 1 || diceCount[5] == 2) return false;

        return true;
    }

    public bool IsStraight(int[] totalCount) {
        bool result = true;

        for (int i = 0; i < totalCount.Length; i++) {
            if (totalCount[i] != 1) {
                result = false;
                break;
            }
        }

        return result;
    }

    public int ScoreCalc(int[] diceCount, int[] totalCount) {
        int score = 0;

        if (scores.Count == 0) {
            InitializeScores();
        }
    
        if (IsStraight(totalCount)) {
            return straightScore;
        }

        // check for 1s
        switch(diceCount[0])
        {
            case 1: score += scores[0][0]; break;
            case 2: score += scores[0][1]; break;
            case 3: score += scores[0][2]; break;
            case 4: score += scores[0][3]; break;
            case 5: score += scores[0][4]; break;
            case 6: score += scores[0][5]; break;
            
            default: break;
        }
        
        // check for 2s
        switch (diceCount[1])
        {
            case 3: score += scores[1][0]; break;
            case 4: score += scores[1][1]; break;
            case 5: score += scores[1][2]; break;
            case 6: score += scores[1][3]; break;
            
            default: break;
        }
        
        // check for 3s
        switch (diceCount[2])
        {
            case 3: score += scores[2][0]; break;
            case 4: score += scores[2][1]; break;
            case 5: score += scores[2][2]; break;
            case 6: score += scores[2][3]; break;
            
            default: break;
        }
        
        // check for 4s
        switch (diceCount[3])
        {
            case 3: score += scores[3][0]; break;
            case 4: score += scores[3][1]; break;
            case 5: score += scores[3][2]; break;
            case 6: score += scores[3][3]; break;
            
            default: break;
        }
        
        // check for 5s
        switch (diceCount[4])
        {
            case 1: score += scores[4][0]; break;
            case 2: score += scores[4][1]; break;
            case 3: score += scores[4][2]; break;
            case 4: score += scores[4][3]; break;
            case 5: score += scores[4][4]; break;
            case 6: score += scores[4][5]; break;
            
            default: break;
        }
        
        // check for 6s
        switch (diceCount[5])
        {
            case 3: score += scores[5][0]; break;
            case 4: score += scores[5][1]; break;
            case 5: score += scores[5][2]; break;
            case 6: score += scores[5][3]; break;
            
            default: break;
        }
        
        // go backwards by 100 if score is 0
        if (score == 0) {
            score = backwardScore;
        }

        return score;
    }

    private void InitializeScores() {
        scores.Add(oneScores);
        scores.Add(twoScores);
        scores.Add(threeScores);
        scores.Add(fourScores);
        scores.Add(fiveScores);
        scores.Add(sixScores);
    }

}