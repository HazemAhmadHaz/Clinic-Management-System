DELETE FROM Applications 
WHERE ApplicantPersonID NOT IN (SELECT PatientID FROM Patients)