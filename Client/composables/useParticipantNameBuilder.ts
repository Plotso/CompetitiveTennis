
import { ParticipantShortOutputModel, ParticipantInfo, MatchShortOutputModel } from '~/types';
export function useParticipantNameBuilder() {
  //const { t } = useI18n();
  
  const buildUsername = (participant: ParticipantShortOutputModel) => {
    if(!participant.players || participant.players.length === 0) 
        return participant.name;
    const playerNames = participant.players
    .map((p) => p.username)
    .join(' ,')

    // If participant is a guest, prepend their name. Otherwise, just return player names.
    return participant.isGuest
    ? `${participant.name}, ${playerNames}`
    : playerNames
  };
  
  const buildUsernameWithRating = (participant: ParticipantShortOutputModel) => {
    if(!participant.players || participant.players.length === 0) 
        return participant.name;
    const playerNames = participant.players
    .map((p) => `${p.username} (${p.playerRating})`)
    .join(' ,')

    // If participant is a guest, prepend their name. Otherwise, just return player names.
    return participant.isGuest
    ? `${participant.name}, ${playerNames}`
    : playerNames
  };
  
  const buildPlainParticipantInfoName = (participant: ParticipantInfo) => {
    if(!participant.players || participant.players.length === 0) 
        return participant.name;
    const playerNames = participant.players
    .map((p) =>`${p.firstName} ${p.lastName}`)
    .join(' ,')

    // If participant is a guest, prepend their name. Otherwise, just return player names.
    return participant.hasGuest
    ? `${participant.name}, ${playerNames}`
    : playerNames
  };
  
  const buildPlainParticipantName = (participant: ParticipantShortOutputModel) => {
    if(!participant.players || participant.players.length === 0) 
        return participant.name;
    const playerNames = participant.players
    .map((p) =>`${p.firstName} ${p.lastName}`)
    .join(' ,')

    // If participant is a guest, prepend their name. Otherwise, just return player names.
    return participant.isGuest
    ? `${participant.name}, ${playerNames}`
    : playerNames
  };

  const buildParticipantInfoNameWithUsername = (participant: ParticipantInfo) => {
    if(!participant.players || participant.players.length === 0) 
        return participant.name;
    const playerNames = participant.players
    .map((p) =>`${p.firstName} ${p.lastName} (${p.username})`)
    .join(' ,')

    // If participant is a guest, prepend their name. Otherwise, just return player names.
    return participant.hasGuest
    ? `${participant.name}, ${playerNames}`
    : playerNames
  };

  const buildParticipantNameWithUsername = (participant: ParticipantShortOutputModel) => {
    if(!participant.players || participant.players.length === 0) 
        return participant.name;
    const playerNames = participant.players
    .map((p) =>`${p.firstName} ${p.lastName} (${p.username})`)
    .join(' ,')

    // If participant is a guest, prepend their name. Otherwise, just return player names.
    return participant.isGuest
    ? `${participant.name}, ${playerNames}`
    : playerNames
  };
  const buildParticipantInfoNameWithUsernameAndRatingAttached = (participant: ParticipantInfo) => {
    if(!participant.players || participant.players.length === 0) 
        return participant.name;
    const playerNames = participant.players
    .map((p) =>`${p.firstName} ${p.lastName} (${p.username} | ${p.playerRating})`)
    .join(' ,')

    // If participant is a guest, prepend their name. Otherwise, just return player names.
    return participant.hasGuest
    ? `${participant.name}, ${playerNames}`
    : playerNames
  };
  const buildPlainParticipantInfoNameWithRatingAttached = (participant: ParticipantInfo) => {
    if(!participant.players || participant.players.length === 0) 
        return participant.name;
    const playerNames = participant.players
    .map((p) =>`${p.firstName} ${p.lastName} (${p.playerRating})`)
    .join(' ,')

    // If participant is a guest, prepend their name. Otherwise, just return player names.
    return participant.hasGuest
    ? `${participant.name}, ${playerNames}`
    : playerNames
  };
  const buildPlainParticipantNameWithRatingAttached = (participant: ParticipantShortOutputModel) => {
    if(!participant.players || participant.players.length === 0) 
        return participant.name;
    const playerNames = participant.players
    .map((p) =>`${p.firstName} ${p.lastName} (${p.username} | ${p.playerRating})`)
    .join(' ,')

    // If participant is a guest, prepend their name. Otherwise, just return player names.
    return participant.isGuest
    ? `${participant.name}, ${playerNames}`
    : playerNames
  };

  const buildHomeParticipantName = (match: MatchShortOutputModel, includeUsername: boolean, includeRating: boolean) => {
    console.log(match);
    if(!match?.homeParticipant) return 'Unknown Player1';
    if(includeRating)
       return includeUsername ? buildParticipantInfoNameWithUsernameAndRatingAttached(match?.homeParticipant) : buildPlainParticipantInfoNameWithRatingAttached(match?.homeParticipant);
    return includeUsername ? buildParticipantInfoNameWithUsername(match?.homeParticipant) : buildPlainParticipantInfoName(match?.homeParticipant);
  };
  const buildAwayParticipantName = (match: MatchShortOutputModel, includeUsername: boolean, includeRating: boolean) => {
    if(!match?.awayParticipant) return 'Unknown Player 2';
    if(includeRating)
       return includeUsername ? buildParticipantInfoNameWithUsernameAndRatingAttached(match?.awayParticipant) : buildPlainParticipantInfoNameWithRatingAttached(match?.awayParticipant);
    return includeUsername ? buildParticipantInfoNameWithUsername(match?.awayParticipant) : buildPlainParticipantInfoName(match?.awayParticipant);
  };

  return { buildUsername, buildUsernameWithRating, buildPlainParticipantName, buildParticipantNameWithUsername, buildPlainParticipantNameWithRatingAttached, buildHomeParticipantName, buildAwayParticipantName, buildParticipantInfoNameWithUsernameAndRatingAttached };
}