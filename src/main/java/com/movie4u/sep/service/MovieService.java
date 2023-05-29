package com.movie4u.sep.service;

import com.movie4u.sep.db.*;
import com.movie4u.sep.db.entity.PeopleEntity;
import com.movie4u.sep.db.entity.RatingEntity;
import com.movie4u.sep.db.entity.StarEntity;
import com.movie4u.sep.mapper.MovieResponseMapper;
import com.movie4u.sep.models.Movie;
import com.movie4u.sep.models.People;
import com.movie4u.sep.models.Rating;
import com.movie4u.sep.models.RatingStatistics;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.PageRequest;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Objects;
import java.util.stream.Collectors;

@Service
public class MovieService {

    @Autowired
    private MovieRepository movieRepository;

    @Autowired
    private MovieResponseMapper mapper;

    @Autowired
    private StarRepository starRepository;

    @Autowired
    private PeopleRepository peopleRepository;


    public List<Movie> findMovieByTitle(String title) {
        var pageable = PageRequest.of(0, 3);
        var movies = movieRepository.findByTitleContainingIgnoreCase(title, pageable);

        var mapped =  movies.orElseThrow()
                .stream().map(m -> mapper.map(m))
                .toList();

        return mapped;

    }

    public ResponseEntity<Movie> findMovieById(int Id) {

        var movie = movieRepository.findById(Id);

        return movie.map(movieEntity ->
                ResponseEntity.ok(mapper.map(movieEntity))).orElseGet(() -> ResponseEntity.badRequest().build());

    }

    public ResponseEntity<List<Rating>> getAllRatingByStarName(String name) {

        var star = peopleRepository.findByNameIgnoreCase(name);
        if (star.isEmpty()) return ResponseEntity.badRequest().build();

        var starEntities = starRepository.findAllByStar(star.get(0));

        return starEntities.map(entities -> ResponseEntity.ok(entities
                        .stream()
                        .filter(Objects::nonNull)
                        .map(StarEntity::getMovie)
                        .filter(Objects::nonNull)
                        .map(m -> mapRating(m.getRatings()))
                        .filter(Objects::nonNull)
                        .collect(Collectors.toList())))
                .orElseGet(() -> ResponseEntity.ok(new ArrayList<>()));

    }

    public ResponseEntity<List<Rating>> getAllRatingByStar(People star) {

        var starEntities = starRepository.findAllByStar(mapToPeopleEntity(star));

        return starEntities.map(entities -> ResponseEntity.ok(entities
                        .stream()
                        .filter(Objects::nonNull)
                        .map(StarEntity::getMovie)
                        .filter(Objects::nonNull)
                        .map(m -> mapRating(m.getRatings()))
                        .filter(Objects::nonNull)
                        .collect(Collectors.toList())))
                .orElseGet(() -> ResponseEntity.ok(new ArrayList<>()));

    }


    public ResponseEntity<List<RatingStatistics>> getAllRatingByYear(Long startYear, Long endYear) {
        var results = movieRepository.findRatingStatisticsInDecade(startYear, endYear);
        List<RatingStatistics> statistics = new ArrayList<>();
        for (Object[] result : results) {
            float rating = (float) result[0];
            long count = (long) result[1];
            RatingStatistics ratingStatistics = new RatingStatistics(rating, count);
            statistics.add(ratingStatistics);
        }

        return ResponseEntity.ok(statistics);
    }


    private Rating mapRating(RatingEntity ratingEntity) {
        Rating rating = new Rating();
        if (ratingEntity == null) return null;
        rating.setMovieId(ratingEntity.getMovieId());
        rating.setRating(ratingEntity.getRating());
        rating.setVotes(ratingEntity.getVotes());

        return rating;
    }

    private PeopleEntity mapToPeopleEntity(People star) {
        PeopleEntity peopleEntity = new PeopleEntity();
        if (star == null) return null;
        peopleEntity.setId(star.getId());
        peopleEntity.setName(star.getName());
        peopleEntity.setBirth(star.getBirth());

        return peopleEntity;
    }

}
