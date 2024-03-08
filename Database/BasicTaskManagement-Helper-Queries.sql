
-- all task groups, not shoing completed task items
SELECT g.Id, g.[Name], g.IsFavorite, i.Id, i.[Name], i.Notes, i.IsImportant, i.IsComplete, i.DueDate, i.CompletedDate, i.CreateDate, i.UpdateDate
FROM TaskGroups g
LEFT JOIN TaskItems i ON (g.Id = i.TaskGroupId)
WHERE i.IsComplete = 0
ORDER BY g.IsFavorite DESC, g.[Name], i.DueDate DESC;

-- task groups showing both completed and not completed task items
SELECT g.Id, g.[Name], g.IsFavorite, i.Id, i.[Name], i.Notes, i.IsImportant, i.IsComplete, i.DueDate, i.CompletedDate, i.CreateDate, i.UpdateDate
FROM TaskGroups g
LEFT JOIN TaskItems i ON (g.Id = i.TaskGroupId)
ORDER BY g.IsFavorite, g.[Name], i.DueDate DESC;

-- task items due today, not showing complete
SELECT * FROM TaskItems ti LEFT JOIN TaskGroups tg ON (ti.TaskGroupId = tg.Id) WHERE CAST(ti.DueDate AS DATE) = CAST(GETDATE() AS DATE) AND ti.IsComplete = 0 ORDER BY ti.Name

-- task items due today, showing complete
SELECT * FROM TaskItems ti LEFT JOIN TaskGroups tg ON (ti.TaskGroupId = tg.Id) WHERE CAST(ti.DueDate AS DATE) = CAST(GETDATE() AS DATE) ORDER BY ti.Name

-- important task items, not showing complete
SELECT * FROM TaskItems ti LEFT JOIN TaskGroups tg ON (ti.TaskGroupId = tg.Id) WHERE ti.IsImportant = 1 AND ti.IsComplete = 0 ORDER BY ti.DueDate DESC, ti.Name

-- important task items, showing complete
SELECT * FROM TaskItems ti LEFT JOIN TaskGroups tg ON (ti.TaskGroupId = tg.Id) WHERE ti.IsImportant = 1 ORDER BY ti.DueDate DESC, ti.Name

-- completed task items
SELECT * FROM TaskItems ti LEFT JOIN TaskGroups tg ON (ti.TaskGroupId = tg.Id) WHERE ti.IsComplete = 1 ORDER BY ti.DueDate DESC, ti.Name

-- task item by id
SELECT * FROM TaskItems ti LEFT JOIN TaskGroups tg ON (ti.TaskGroupId = tg.Id) WHERE ti.Id = 7;


