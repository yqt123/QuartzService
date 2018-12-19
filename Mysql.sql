-- use quartz_esb

CREATE TABLE esb_settings(
`id` INT auto_increment COMMENT '自增ID',
`Key` VARCHAR(200) NOT NULL,
`Value` VARCHAR(200) CHARACTER SET utf8,
PRIMARY KEY (id))
ENGINE=InnoDB;
