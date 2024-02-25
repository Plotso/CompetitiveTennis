export interface Result<TData> {
  isSuccess: boolean;
  errors: string[];
  data: TData;
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

  export enum MatchOutcome {
    Unknown,
    Participant1Won,
    Participant2Won,
    Draw,
  }

  export enum OutcomeCondition {
    Points,
    Injury,
    Disqualification,
    Withdrawal,
  }

  export enum TournamentStage {
    Unknown,
    GroupStage,
    KnockoutStage,
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
  }
  
  export interface MatchPeriodOutputModel {
    id: number;
    set: number;
    game: number;
    status: EventStatus;
    winner: MatchOutcome;
    server: EventActor;
    isTiebreak: boolean,
    scores: ScoreShortOutputModel[];
  }
  
  export interface ScoreShortOutputModel {
    id: number;
    periodPointNumber: number;
    participant1Points: string;
    participant2Points: string;
    pointWinner: MatchOutcome
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

  export interface TournamentQuery {
    keyword?: string;
    hasEntryFee?: boolean | null;
    hasPrize?: boolean | null;
    sortOptions?: SortOptions;
    surface?: Surface | null;
    tournamentType?: TournamentType | null;
    isIndoor?: boolean | null;
    dateRangeFrom?: Date | null;
    dateRangeUntil?: Date | null;
    organiserId?: number | null;
    participantIds?: number[] | null;
    page?: number;
    itemsPerPage?: number;
  }
  
  export interface AvenueQuery {
    keyword?: string;
    city?: string;
    country?: string;
    sortOptions?: SortOptions;
    surface?: Surface | null;
    courtType?: CourtType | null;
    page?: number;
    itemsPerPage?: number;
  }
