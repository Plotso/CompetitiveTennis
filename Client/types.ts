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

  export interface TournamentOutputModel {
    Id: number;
    Title: string;
    Rules: string;
    Description: string;
    Type: TournamentType;
    Surface: Surface;
    EntryFee: number | null;
    Prize: number | null;
    CourtsAvailable: number;
    MinParticipants: number;
    MaxParticipants: number;
    MatchWonPoints: number | null;
    SetWonPoints: number | null;
    GameWonPoints: number | null;
    StartDate: Date;
    EndDate: Date;
    CreatedOn: Date;
    ModifiedOn: Date;
    Avenue: AvenueShortOutputModel;
    Organiser: AccountOutputModel;
    Participants: ParticipantShortOutputModel[];
    Matches: MatchOutputModel[];
  }

  export interface AvenueShortOutputModel {
    Id: number;
    Name: string;
    Location: string;
    City: string;
    Country: string;
    Details: string;
    IsVerified: boolean;
    IsActive: boolean;
  }

  export interface AccountOutputModel {
    Id: number;
    Username: string;
    FirstName: string;
    LastName: string;
    PlayerRating: number;
    Participations: ParticipantShortOutputModel[];
    OrganisedTournaments: TournamentShortInfoOutput[];
  }

  export interface ParticipantShortOutputModel {
    Id: number;
    Name: string | null;
    Points: number | null;
    IsGuest: boolean;
    Players: AccountShortOutputModel[];
  }

  export interface TournamentShortInfoOutput {
    Id: number;
    Title: string;
  }

  export interface AccountShortOutputModel {
    Id: number;
    Username: string;
    FirstName: string;
    LastName: string;
    PlayerRating: number;
  }

  export interface MatchOutputModel {
    Id: number;
    StartDate: Date;
    EndDate: Date;
    Participant1Id: number;
    MatchWonPoints: number | null;
    SetWonPoints: number | null;
    GameWonPoints: number | null;
    Stage: TournamentStage;
    Details: string | null;
    Status: EventStatus;
    Outcome: MatchOutcome;
    Scores: ScoreShortOutputModel[];
    Participants: ParticipantShortOutputModel[];
  }

  export interface ScoreShortOutputModel {
    Id: number;
    Set: number;
    Game: number;
    Participant1Points: string;
    Participant2Points: string;
    Status: EventStatus;
  }


