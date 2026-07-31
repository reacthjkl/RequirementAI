export interface ArtifactWordCount {
  id: string;
  title: string;
  words: number;
}

export interface ProjectWordCount {
  projectId: string;
  projectName: string;
  totalWords: number;
  averageWordsPerPersona: number;
  averageWordsPerScenario: number;
  averageWordsPerUserStory: number;
  wordsPerPersona: ArtifactWordCount[];
  wordsPerScenario: ArtifactWordCount[];
  wordsPerUserStory: ArtifactWordCount[];
}
