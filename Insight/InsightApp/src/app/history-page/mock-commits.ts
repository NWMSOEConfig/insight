export const mockCommits = [
  {
    id: 'B93ASD3',
    timestamp: '2023-01-13T09:12:03.909Z',
    message: 'Creating settings for a new client in California',
    user: 'Guy',
    batch: [
      {
        settingName: 'ActionItemListDaysSinceCompletion',
        oldValue: '500',
        newValue: '100',
      },
    ],
  },
  {
    id: 'A92CE2L',
    timestamp: '2023-01-09T01:56:47.909Z',
    message: 'Modified settings per client request',
    user: 'Lad',
    batch: [
      {
        settingName: 'ActivateStudyPlanNoteActivityID',
        oldValue: '100',
        newValue: '25',
      },
      {
        settingName: 'ShowLogoQRSDashboardRating',
        oldValue: 'True',
        newValue: 'false',
      },
      {
        settingName: 'ShowProcessStipendsButton',
        oldValue: 'true',
        newValue: 'False',
      },
      {
        settingName: 'QrisDisplayFaStarIcon',
        oldValue: 'false',
        newValue: 'True',
      },
      {
        settingName: 'ComplianceMonitorProgramTypeId',
        oldValue: '14',
        newValue: '32',
      },
      {
        settingName: 'ConferenceRegistrationSelectionsReportName',
        oldValue: 'OldConferenceRegistrationSelections',
        newValue: 'NewConferenceRegistrationSelections',
      },
    ],
  },
  {
    id: 'C7C8L3Y',
    timestamp: '2022-12-15T17:27:56.209Z',
    message: 'Modified all these settings because they were wrong before',
    user: 'Man',
    batch: [
      {
        settingName: 'ConsultantRequiresCurrentEndorsement',
        oldValue: 'FALSE',
        newValue: 'true',
      },
      {
        settingName: 'ConsultingEventEndDateEnabled',
        oldValue: 'False',
        newValue: 'TRUE',
      },
      {
        settingName: 'CourseCatalogCoursePageSize',
        oldValue: '25',
        newValue: '97',
      },
      {
        settingName: 'IndividualTrainingRequestRequireAgency',
        oldValue: 'True',
        newValue: 'false',
      },
      {
        settingName: 'NCConsultingEventApproval',
        oldValue: '9',
        newValue: '18',
      },
      {
        settingName: 'NAConferenceStatus',
        oldValue: '57',
        newValue: '92',
      },

      {
        settingName: 'NavQIAdminuserQrisRoleIds',
        oldValue: '1,6,11',
        newValue: '3,6,9',
      },
    ],
  },
];

for (let i = mockCommits.length; i < 30539; i++) {
  mockCommits.push({
    id: 'Commit ' + (i + 1),
    timestamp: '2022-01-02T00:00:00.009Z',
    message: 'Commit Message',
    user: 'Guy ' + (i + 1),
    batch: [
      {
        settingName: 'Setting',
        oldValue: 'Old',
        newValue: 'New',
      },
    ],
  });
}
