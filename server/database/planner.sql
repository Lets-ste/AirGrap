
SELECT * FROM timetable.task;
SELECT timetable.delete_job('cleanup-sessions');
SELECT timetable.add_job(
    job_name     => 'cleanup-sessions',
    job_schedule => '*/30 * * * *',
    job_command  => $$DELETE FROM sessions WHERE last_activity_at < NOW() - INTERVAL '30 minutes'$$
);
