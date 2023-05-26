package com.movie4u.sep.db;

import com.movie4u.sep.db.entity.TopListMovie;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface TopListMovieRepository extends JpaRepository<TopListMovie, Long> {
    Optional<List<TopListMovie>> findAllByUsername(String username);
}
