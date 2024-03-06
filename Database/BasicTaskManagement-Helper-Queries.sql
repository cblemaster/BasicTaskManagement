

SELECT g.Id, g.[Name], g.IsFavorite, i.Id, i.[Name], i.Notes, i.IsImportant, i.IsComplete, i.DueDate, i.CompletedDate, i.CreateDate, i.UpdateDate
FROM TaskGroups g
LEFT JOIN TaskItems i ON (g.Id = i.TaskGroupId)
-- WHERE i.IsImportant = 1
-- WHERE i.IsComplete = 1
-- WHERE i.DueDate = CAST(GETDATE() AS DATE)
ORDER BY g.[Name], i.DueDate DESC;
-- ORDER BY i.DueDate DESC;


-- SELECT * FROM Groups g;
--SELECT * FROM items i ORDER BY i.DueDate DESC;