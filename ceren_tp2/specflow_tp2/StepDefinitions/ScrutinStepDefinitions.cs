using ceren_tp2;
using NUnit.Framework;
using TechTalk.SpecFlow;
using System.Collections.Generic;
using System.Linq;

namespace specflow_tp2.StepDefinitions
{
    [Binding]
    public sealed class ScrutinStepDefinitions
    {
        private bool isVoteOpen;
        private List<Candidate>? candidates;
        private List<Candidate>? firstRoundResults;
        private Scrutin scrutin;
        private Candidate? winner;
        private bool requiresSecondRound;
        private int blankVotes;

        public ScrutinStepDefinitions()
        {
            scrutin = new Scrutin();
        }

        #region Given

        [Given("the voting is closed")]
        public void GivenTheVoteIsClosed()
        {
            isVoteOpen = false;
        }

        [Given("the candidates and their votes are")]
        public void GivenTheCandidatesAndVotes(Table table)
        {
            candidates = table.Rows.Select(row => new Candidate
            {
                name = row["name"],
                votes = int.Parse(row["votes"])
            }).ToList();
        }

        [Given("the first round results are")]
        public void GivenTheFirstRoundResultsAre(Table table)
        {
            firstRoundResults = table.Rows.Select(row => new Candidate
            {
                name = row["name"],
                votes = int.Parse(row["votes"]),
                votePercentage = double.Parse(row["votePercentage"])
            }).ToList();
        }

        [Given("there are (.*) blank votes")]
        public void GivenThereAreBlankVotes(int blankVotesCount)
        {
            blankVotes = blankVotesCount;
        }

        #endregion

        #region When

        [When(@"we count the votes")]
        public void WhenWeCountTheVotes()
        {
            (winner, candidates, requiresSecondRound) = scrutin.CalculateFirstRoundResult(candidates, blankVotes);
        }

        [When(@"the second round results are calculated")]
        public void WhenTheSecondRoundResultsAreCalculated()
        {
            if (firstRoundResults == null)
            {
                throw new ArgumentNullException(nameof(firstRoundResults));
            }

            winner = scrutin.CalculateSecondRoundResults(firstRoundResults);
        }

        #endregion

        #region Then

        [Then(@"the winner should be (.*)")]
        public void ThenTheWinnerShouldBe(string expectedWinner)
        {
            Assert.AreEqual(expectedWinner, winner?.name);
        }

        [Then(@"the winner count should be")]
        public void ThenTheWinnerCountShouldBe(Table table)
        {
            foreach (var row in table.Rows)
            {
                var candidate = candidates.FirstOrDefault(c => c.name == row["name"]);
                Assert.AreEqual(int.Parse(row["votes"]), candidate?.votes);
                Assert.AreEqual(Math.Round(double.Parse(row["votePercentage"]), 2), Math.Round(candidate?.votePercentage ?? 0, 2));
            }
        }

        [Then(@"there should be a second round")]
        public void ThenThereShouldBeASecondRound()
        {
            Assert.IsTrue(requiresSecondRound);
        }

        [Then(@"the remaining candidates should be")]
        public void ThenTheRemainingCandidatesShouldBe(Table table)
        {
            foreach (var row in table.Rows)
            {
                var candidate = candidates.First(c => c.name == row["name"]);
                Assert.AreEqual(int.Parse(row["votes"]), candidate.votes);
                Assert.AreEqual(Math.Round(double.Parse(row["votePercentage"]), 2), Math.Round(candidate.votePercentage, 2));
            }
        }

        [Then(@"the vote counts should be")]
        public void ThenTheVoteCountsShouldBe(Table table)
        {
            foreach (var row in table.Rows)
            {
                var candidate = firstRoundResults?.First(c => c.name == row["name"]);
                Assert.AreEqual(int.Parse(row["votes"]), candidate?.votes);
                Assert.AreEqual(Math.Round(double.Parse(row["votePercentage"]), 2), Math.Round(candidate?.votePercentage ?? 0, 2));
            }
        }

        [Then(@"there should be a tie")]
        public void ThenThereShouldBeATie()
        {
            Assert.IsNull(winner);
        }

        #endregion
    }
}
