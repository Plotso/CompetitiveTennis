export interface Result<TData> {
  isSuccess: boolean;
  errors: string[];
  data: TData;
}

export interface SearchOutputModel<TRecord> {
  results: TRecord[];
  page: number;
  total: number;
}


/// Auth
export namespace Auth {
  export interface ChangeNamesInputModel {
    firstName: string;
    lastName: string;
  }

  export interface ChangePasswordInputModel {
    currentPassword: string;
    newPassword: string;
  }

  export interface FullUserOutputModel {
    token: string;
    email: string;
    username: string;
    firstName: string;
    lastName: string;
    hasAdministrativeRights: boolean;
  }

  export interface RegisterInputModel {
    email: string;
    username: string;
    firstName: string;
    lastName: string;
    password: string;
  }

  export interface UserInputModel {
    loginInfo: string;
    password: string;
    emailLogin: boolean;
  }

  export interface UserOutputModel {
    token: string;
    username: string;
    hasAdministrativeRights: boolean;
  }
}

/// Tournaments

 export enum TournamentType {
    Singles,
    Doubles,
    Teams,
  }

  export enum Surface {
    Other,
    Hard,
    Clay,
    Grass,
    Carpet,
    Asphalt,
    Acrylic,
    Wood,
    BlueClay,
    ArtificialGrass,
  }

  export enum EventStatus {
    NotStarted,
    InProgress,
    Ended,
  }

  export enum EventActor {
    Unknown,
    Home,
    Away,
  }
  export enum MatchPeriodOutcome {
    NoOutcome,
    ParticipantOne,
    ParticipantTwo
  }

  export enum MatchOutcome {
    Unknown,
    Participant1Won,
    Participant2Won,
    Draw
  }

  export enum OutcomeCondition {
    Points,
    Injury,
    Disqualification,
    Withdrawal,
  }

  export enum TournamentStage {
    Unknown,
    Qualification,
    RoundOf128,
    RoundOf64,
    RoundOf32,
    RoundOf16,
    QuarterFinal,
    SemiFinal,
    Final,
    LeagueMatch,
    GroupStage,
  }

  export enum CourtType {
    Outdoor,
    Indoor,
  }

  export enum SortOptions {
    CreatedDescending,
    CreatedAscending,
    UpdatedAscending,
    UpdatedDescending
  }

  export enum AccountSortOptions {
    SinglesRatingDescending,
    SinglesRatingAscending,
    DoublesRatingDescending,
    DoublesRatingAscending
  }

  export interface TournamentOLDOutputModel {
    id: number;
    title: string;
    rules: string;
    description: string;
    type: TournamentType;
    surface: Surface;
    entryFee: number | null;
    prize: number | null;
    courtsAvailable: number;
    minParticipants: number;
    maxParticipants: number;
    matchWonPoints: number | null;
    setWonPoints: number | null;
    gameWonPoints: number | null;
    startDate: Date;
    endDate: Date;
    createdOn: Date;
    modifiedOn: Date;
    avenue: AvenueShortOutputModel;
    organiser: AccountOutputModel;
    participants: ParticipantShortOutputModel[];
    matches: MatchOutputModel[];
  }

  export interface TournamentOutputModel {
    id: number;
    title: string;
    rules: string;
    description: string;
    type: TournamentType;
    surface: Surface;
    entryFee: number | null;
    prize: number | null;
    courtsAvailable: number;
    minParticipants: number;
    maxParticipants: number;
    matchWonPoints: number | null;
    setWonPoints: number | null;
    gameWonPoints: number | null;
    startDate: Date;
    endDate: Date;
    createdOn: Date;
    modifiedOn: Date;
    avenue: AvenueShortOutputModel;
    organiser: AccountOutputModel;
    participants: ParticipantShortOutputModel[];
    matches: MatchOutputModel[];
  }

  export interface SlimTournamentOutputModel {
    id: number;
    title: string;
    rules: string;
    description: string;
    type: TournamentType;
    surface: Surface;
    entryFee: number | null;
    prize: number | null;
    courtsAvailable: number;
    minParticipants: number;
    maxParticipants: number;
    matchWonPoints: number | null;
    setWonPoints: number | null;
    gameWonPoints: number | null;
    startDate: Date;
    endDate: Date;
    createdOn: Date;
    modifiedOn: Date;
    avenue: AvenueShortOutputModel;
    organiser: AccountOutputModel;
    participants: ParticipantShortOutputModel[];
    matches: MatchShortOutputModel[];
  }
  export interface AvenueShortOutputModel {
    id: number;
    name: string;
    location: string;
    city: string;
    country: string;
    details: string;
    isVerified: boolean;
    isActive: boolean;
  }
  
  export interface AccountOutputModel {
    id: number;
    username: string;
    firstName: string;
    lastName: string;
    playerRating: number;
    doublesRating: number;
    participations: ParticipantShortOutputModel[];
    organisedTournaments: TournamentShortInfoOutput[];
  }
  
  export interface ParticipantShortOutputModel {
    id: number;
    name: string | null;
    points: number | null;
    isGuest: boolean;
    players: AccountShortOutputModel[];
  }
  
  export interface ParticipantInfo {
    id: number;
    name: string | null;
    points: number | null;
    hasGuest: boolean;
    players: AccountShortOutputModel[];
    team: TeamShortOutputModel;
  }
  
  export interface TeamShortOutputModel {
    id: number;
    name: string | null;
  }
  
  export interface TournamentShortInfoOutput {
    id: number;
    title: string;
  }
  
  export interface AccountShortOutputModel {
    id: number;
    username: string;
    firstName: string;
    lastName: string;
    playerRating: number;    
    doublesRating: number;
  }
  
  export interface MatchOutputModel {
    id: number;
    startDate: Date;
    endDate: Date;
    participant1Id: number;
    matchWonPoints: number | null;
    setWonPoints: number | null;
    gameWonPoints: number | null;
    stage: TournamentStage;
    details: string | null;
    status: EventStatus;
    outcome: MatchOutcome;
    outcomeCondition: OutcomeCondition | null;
    matchPeriods: MatchPeriodOutputModel[];
    participants: ParticipantShortOutputModel[];
  }
  
  export interface MatchShortOutputModel {
    id: number;
    startDate: Date;
    homePredecesorMatchId: number;
    awayPredecesorMatchId: number;
    matchWonPoints: number | null;
    setWonPoints: number | null;
    gameWonPoints: number | null;
    stage: TournamentStage;
    details: string | null;
    status: EventStatus;
    outcome: MatchOutcome;
    outcomeCondition: OutcomeCondition | null;
    homeParticipant: ParticipantInfo;
    awayParticipant: ParticipantInfo;
    matchPeriods: MatchPeriodOutputModel[];
    tournamentId: number;
    results: MatchResultsOutputModel | null
  }
  
  export interface MatchPeriodOutputModel {
    id: number;
    set: number;
    game: number;
    status: EventStatus;
    winner: MatchPeriodOutcome;
    server: EventActor;
    isTiebreak: boolean;
    scores: ScoreShortOutputModel[];
  }
  
  export interface ScoreShortOutputModel {
    id: number;
    periodPointNumber: number;
    participant1Points: string;
    participant2Points: string;
    pointWinner: MatchPeriodOutcome,
    isWinningPoint: boolean;
  }

  export interface MatchPeriodInputModel {
    set: number;
    game: number;
    status: EventStatus;
    server: EventActor;
    winner: MatchOutcome;
    isTiebreak: boolean;
    scores: ScoreInputModel[];
  }
  
  export interface ScoreInputModel {
    periodPointNumber: number;
    participant1Points: string;
    participant2Points: string;
    pointWinner: MatchOutcome;
    isWinningPoint: boolean;
  }

  export interface MatchOutcomeInputModel {
    outcomeCondition: OutcomeCondition | null;
    matchPeriods: MatchPeriodInputModel[];
  }

  export interface CourtsInfo {
    surface: Surface;
    availableCourtsByType: { [key in CourtType]: number };
  }
  
  export interface TournamentShortInfoOutput {
    id: number;
    title: string;
    type: TournamentType;
    surface: Surface;
    entryFee: number | null;
    prize: number | null;
    startDate: Date;
    endDate: Date;
  }
  
  export interface AvenueOutputModel {
    id: number;
    name: string;
    location: string;
    city: string;
    country: string;
    details: string;
    isVerified: boolean;
    isActive: boolean;
    courts: CourtsInfo[];
    tournaments: TournamentShortInfoOutput[];
  }

  export interface TournamentInputModel {
    title: string;
    rules: string;
    description: string;
    type: TournamentType;
    surface: Surface;
    entryFee?: number | null;
    prize?: number | null;
    courtsAvailable: number;
    minParticipants: number;
    maxParticipants: number;
    matchWonPoints?: number | null;
    setWonPoints?: number | null;
    gameWonPoints?: number | null;
    isIndoor: boolean;
    isLeague: boolean;
    startDate: Date;
    endDate: Date;
    avenueId: number;
  }

  export interface AvenueInputModel {
    name: string;
    location: string;
    city: string;
    country: string;
    details: string;
    courts: CourtsInfo[];
  }
  
  
  export interface ParticipantInputModel {
    name?: string | null;
    points?: number | null;
    isGuest: boolean;
    tournamentId: number;
    teamId?: number | null;
  }
  
  export interface MultiParticipantInputModel {
    participantInfo: ParticipantInputModel;
    accounts: number[];
    includeCurrentUser: boolean;
  }

  export interface PageQuery {
    page?: number;
    itemsPerPage?: number;
  }

  export interface TournamentQuery extends PageQuery {
    keyword?: string;
    hasEntryFee?: boolean | null;
    hasPrize?: boolean | null;
    city?: string;
    sortOptions?: SortOptions;
    surface?: Surface | null;
    tournamentType?: TournamentType | null;
    isIndoor?: boolean | null;
    dateRangeFrom?: Date | null;
    dateRangeUntil?: Date | null;
    isOngoingAtDateTime?: Date | null;
    organiserId?: number | null;
    participantIds?: number[] | null;
    participantUsernames?: string[] | null;
  }
  
  export interface AvenueQuery extends PageQuery  {
    keyword?: string;
    city?: string;
    country?: string;
    sortOptions?: SortOptions;
    surface?: Surface | null;
    courtType?: CourtType | null;
  }

  export interface MatchQuery extends PageQuery {
    participantUsername?: string;
    participantName?: string;
    isParticipantWinner?: boolean | null;
    status?: EventStatus | null;
    outcomeCondition?: OutcomeCondition | null;
    tournamentType?: TournamentType | null;
    dateRangeFrom?: Date | null;
    dateRangeUntil?: Date | null;
    sortOptions?: SortOptions;
    surface?: Surface | null;
    tournamentStage?: TournamentStage | null;
  }
  
  export interface AccountQuery extends PageQuery {
    keyword?: string;
    sortOptions?: SortOptions;
    additionalSortOptions?: AccountSortOptions;
  }

  export interface MatchCustomConditionResultInputModel {
    outcomeCondition: OutcomeCondition;
    matchOutcome: MatchPeriodOutcome;
  }

  export interface MatchResultsOutputModel {
    setResults: SetResultOutput[];
  }
  
  export interface SetResultOutput {
    setNumber: number; 
    winner: MatchPeriodOutcome;
    homeSideGamesWon: number;
    awaySideGamesWon: number;
  }
  
  

  export interface AccountStats {
    playerRating: number;
    doublesRating: number;
    matchesPlayed: number;
    tournamentsPlayed: number;
    tournamentsWon: number;
  }
  
  
