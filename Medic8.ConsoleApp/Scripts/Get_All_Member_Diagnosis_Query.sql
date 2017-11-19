WITH 
  -- 
  -- REQ. 3) b. 
  --      Most Severe Diagnosis ID and Description should be the diagnosis 
  --      with the lowest Diagnosis ID for each Member/Category.
  --
  CTE_SEVERE_DIAGANOSIS (MemberId, MostSevereDiagnosisId)
  AS
  ( SELECT 
      md.MemberId
      , MIN(md.DiagnosisId)
    FROM 
      MemberDiagnosis md 
	GROUP BY md.MemberId
),
  --
  -- REQ. 3) c. 
  --      Is Most Severe Category should identify the lowest Category ID for each Member
  --
  CTE_MOST_SEVERE_CATEGORY (MemberId, MostSevereCategoryId)
  AS
  ( SELECT 
      md.MemberId
      , MIN(dc.DiagnosisCategoryID)
    FROM
	  MemberDiagnosis md 
      LEFT JOIN DiagnosisCategoryMap dcm ON dcm.DiagnosisID = md.DiagnosisID
      LEFT JOIN DiagnosisCategory dc ON dcm.DiagnosisCategoryID = dc.DiagnosisCategoryID
    GROUP BY md.MemberId
)
  --
  -- REQ. 3) a. 
  --      Please include the following fields: Member ID, First Name, Last Name, 
  --      Most Severe Diagnosis ID, Most Severe Diagnosis Description, Category ID,
  --      Category Description, Category Score and Is Most Severe Category.
  -- 
SELECT 
  m.MemberId AS 'Member Id'
  , m.FirstName as 'First Name'
  , m.LastName AS 'Last Name'
  , sd.MostSevereDiagnosisId AS ' Most Severe Diagnosis Id'
  , d.DiagnosisDescription AS 'Most Severe Diagnosis Description'
  , msc.MostSevereCategoryId AS 'Category ID'
  , dc.CategoryDescription  AS 'Category Description'
  , dc.CategoryScore AS 'Category Score'
  --
  -- REQ. 3) c. 
  --      (please set this to 1 for Members without corresponding Categories as well).
  , COALESCE(msc.MostSevereCategoryId,1) as 'Is Most Severe Category'
FROM
  Member m 
    --
    -- REQ. 3) d.
    --      This query should return one result for each Member/Category.
    --
	LEFT JOIN CTE_SEVERE_DIAGANOSIS sd ON m.MemberId = sd.MemberId
    LEFT JOIN Diagnosis d ON sd.MostSevereDiagnosisId =  d.DiagnosisID
	LEFT JOIN CTE_MOST_SEVERE_CATEGORY msc ON m.MemberId = msc.MemberId
    LEFT JOIN DiagnosisCategory dc ON msc.MostSevereCategoryId = dc.DiagnosisCategoryID