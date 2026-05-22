import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import {
  faBookOpen,
  faBook,
  faFileLines,
  faFile,
  faFolderOpen,
  faGear,
  faHouse,
  faTableColumns,
  faUser,
  faUsers,
} from '@fortawesome/free-solid-svg-icons';

export type EntityIconKey =
  | 'project'
  | 'overview'
  | 'persona'
  | 'scenario'
  | 'userStory'
  | 'board'
  | 'settings';

export type EntityCollectionIconKey = 'personas' | 'scenarios' | 'userStories';

export const ENTITY_ICONS: Record<EntityIconKey, IconDefinition> = {
  project: faFolderOpen,
  overview: faHouse,
  persona: faUser,
  scenario: faFile,
  userStory: faBook,
  board: faTableColumns,
  settings: faGear,
};

export const ENTITY_COLLECTION_ICONS: Record<EntityCollectionIconKey, IconDefinition> = {
  personas: faUsers,
  scenarios: faFileLines,
  userStories: faBookOpen,
};
