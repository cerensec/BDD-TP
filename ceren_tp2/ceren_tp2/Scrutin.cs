namespace ceren_tp2
{
    public class Scrutin
    {
        private bool isVoteOpen;
        private List<Candidate> candidates = new List<Candidate>();
        private Candidate winner;

        public (Candidate? winner, List<Candidate> allCandidates , bool requiresSecondRound) CalculateFirstRoundResult(List<Candidate> candidates, int blankVotes = 0)
        {
            var totalVotes = candidates.Sum(c => c.votes) + blankVotes;

            foreach (var candidate in candidates)
            {
                candidate.votePercentage = CalculatePercentage(candidate.votes, totalVotes);
            }

            if (blankVotes > 0)
            {
                double blankVotePercentage = CalculatePercentage(blankVotes, totalVotes);
                candidates.Add(new Candidate { name = "Blank", votes = blankVotes, votePercentage = blankVotePercentage });
            }

            winner = candidates.OrderByDescending(c => c.votePercentage).First();

            if(winner.votePercentage < 50.0)
            {
                var sortedCandidates = candidates.OrderByDescending(c => c.votes).ToList();
                var secondRoundCandidates = sortedCandidates.Take(2).ToList();

                if (sortedCandidates[1].votes == sortedCandidates[2].votes)
                {
                    secondRoundCandidates.Add(sortedCandidates[2]);
                }

                return (null, secondRoundCandidates, true);

            }

            return (winner, candidates, false);
        }

        public Candidate? CalculateSecondRoundResults(List<Candidate> secondRoundCandidates)
        {
            var totalVotes = secondRoundCandidates.Sum(c => c.votes);

            foreach (var candidate in secondRoundCandidates)
            {
                candidate.votePercentage = Math.Round((double)candidate.votes / totalVotes * 100, 2);
            }

            if (secondRoundCandidates[0].votePercentage == secondRoundCandidates[1].votePercentage)
            {
                return null;
            }

            return secondRoundCandidates.OrderByDescending(c => c.votePercentage).First();

        }

        private double CalculatePercentage(int votes, int totalVotes)
        {
            return Math.Round((double)votes / totalVotes * 100, 1, MidpointRounding.AwayFromZero);
        }

    }

    public class Candidate
    {
        public string name { get; set; }
        public int votes { get; set; }
        public double votePercentage { get; set; }
    }
}
