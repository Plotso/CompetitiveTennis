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

  export enum MatchOutcome {
    Unknown,
    Participant1Won,
    Participant2Won,
    Draw,
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
    scores: ScoreShortOutputModel[];
    participants: ParticipantShortOutputModel[];
  }
  
  export interface ScoreShortOutputModel {
    id: number;
    set: number;
    game: number;
    participant1Points: string;
    participant2Points: string;
    status: EventStatus;
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
  


