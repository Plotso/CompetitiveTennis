
import { MatchShortOutputModel, TournamentStage, MatchPeriodOutcome } from '~/types';
export function useMatchHelper() {
  //const { t } = useI18n();
  
  const getStageString = (stage: string) => {
    switch(stage){
        case "RoundOf16":
            return "1/8 Final";
        case "QuarterFinal":
            return "1/4 Final";
        case "SemiFinal":
            return "Semi Final";
        default:
            return stage;
    };
  }
      
  const getStageStringFromMatch = (match: MatchShortOutputModel) => {
    switch(match.stage){
        case TournamentStage[TournamentStage.RoundOf16]:
            return "1/8 Final";
        case TournamentStage[TournamentStage.QuarterFinal]:
            return "1/4 Final";
        case TournamentStage[TournamentStage.SemiFinal]:
            return "Semi Final";
        default:
            return match.stage;
    };
  }
      
  const getMinimalStageStringFromMatch = (match: MatchShortOutputModel) => {
    switch(match.stage){
        case TournamentStage[TournamentStage.RoundOf128]:
            return "R128";
        case TournamentStage[TournamentStage.RoundOf64]:
            return "R64";
        case TournamentStage[TournamentStage.RoundOf32]:
            return "R32";
        case TournamentStage[TournamentStage.RoundOf16]:
            return "R16";
        case TournamentStage[TournamentStage.QuarterFinal]:
            return "QF";
        case TournamentStage[TournamentStage.SemiFinal]:
            return "SF";
        case TournamentStage[TournamentStage.Final]:
            return "F🏆";
        default:
            return match.stage;
    };
  }

  const getResult = (match: MatchShortOutputModel): string => {
    if(!match || !match.results || !match.results.setResults || match.results.setResults.length == 0)
      return "";
    var result = match.results.setResults.reduce(
      (result, set) => {
        if (set.winner === MatchPeriodOutcome[MatchPeriodOutcome.ParticipantOne]) {
          result.participant1Wins++;
        } else if (set.winner === MatchPeriodOutcome[MatchPeriodOutcome.ParticipantTwo]) {
          result.participant2Wins++;
        }
        return result;
      },
      { participant1Wins: 0, participant2Wins: 0 } // Initial accumulator
    );
    return `${result.participant1Wins}:${result.participant2Wins}`
  }

  const isMatchWinner = (match: MatchShortOutputModel, side: 'home' | 'away') => {
    return (side === 'home' && match.outcome === MatchPeriodOutcome[MatchPeriodOutcome.ParticipantOne]) || (side === 'away' && match.outcome === MatchPeriodOutcome[MatchPeriodOutcome.ParticipantTwo]);
  };

  return { getStageString, getStageStringFromMatch, getMinimalStageStringFromMatch, getResult, isMatchWinner };
}